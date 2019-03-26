
local base = require("module.battle.unit.base_unit")
local this = class("creature",base)
local trace = require("module.battle.battle_trace")

local transform = require("module.battle.unit.component.transform")
local state_ctrl = require("module.battle.unit.component.state_control")
local property = require("module.battle.unit.component.property")
local buffcont = require("module.battle.unit.component.buff_container")
local behavior_tree = require("module.battle.unit.behavior_tree.behavior_tree")
local entire_skill = require("module.battle.skill.entire_skill")
local pack_data = require("module.battle.skill.utils.pack_database")
local buff = require("module.battle.skill.raw_skill.buff")

function this:ctor( sess,data,uid ,struct_uid)
    self.sess = sess
    self.data = data
    self.id = data.id
    -- type: 0,creature;1,structure;-1,hero
    self.type = data.type
    -- genus: 1,ground 2,fly
    self.genus = data.genus
    -- opposite_type: 1,only ground 2,only fly 3,ground and sky
    self.opposite_type =data.opposite_type
    self.uid = uid
    self.card_uid = data.card_uid
    -- only structure use
    self.struct_uid = struct_uid
    if data.type < 0 then
        self.config = config_mng.get_hero_config(data)
    else
        self.config = config_mng.get_unit_config(self.id)
    end

    if self.type == 0 then
        if data.side == 1 then
            self.ai_vo = require("config.ai_config.normal_defence_ai")
        else
            self.ai_vo = require("config.ai_config.normal_ai")
        end
    else
        self.ai_vo = require("config.ai_config.normal_structure_ai")
    end
   

   self.delta = 0
   self.idle_time = 0

   self.name = data.name
   
   self.side = data.side
    
    self:init()

    -- threat_value to every enemy: key is uid,  ps: value will not be cleared
    self.threat_value = {}
end

local function make_event(name)
    this[name] = function(obj, src) 
      obj:dispatch(name, src)
    end
end

make_event("pre_normal_damage")
make_event("pre_normal_damaged")
make_event("pre_damage")
make_event("pre_damaged")
make_event("post_damage")
make_event("post_damaged")
make_event("pre_heal")
make_event("pre_healed")
make_event("post_heal")
make_event("post_healed")
make_event("on_kill")
make_event("on_die")
make_event("on_attack")

function this:init(  )
    -- init data
    self.alive = 0
    -- threat value
    self.threat_value = {}
    -- wait for appear action
    self.appeared = 0
    -- init room
    self.last_location = -1
    self.arrived_rooms = {}
    --if self.data.init_room ~= -1 then
    self.location = self.data.init_room
    local init_pos = self.sess.battle_map:get_room_center(self.data.init_room)
    self.data.init_x = init_pos.X
    self.data.init_y = init_pos.Y
    table.insert( self.arrived_rooms,self.location)
    --else
    --self.location = -1
    --end
    


    local data = self.data
 
    if self.type == 0 or self.type == -1 then
        self.entity = self.sess.map:CreateEntity(self.data.id,self.uid,self.side,self.data.init_x,self.data.init_y,-1)
    else
        self.entity = self.sess.map:CreateEntity(self.data.id,self.uid,self.side,self.data.init_x,self.data.init_y,self.struct_uid)
    end


 
   self.live_time = data.live_time
   self.property = property.new(self,property.unpack_prop(data))
   self.buffcont = buffcont.new(self)
   self.transform = transform.new(self,data)
   self.statectrl = state_ctrl.new(self)
   self.betree = behavior_tree:build(self,self.ai_vo)
    
   -- attack cache
   self.attack_process = 0
   self.skill_process = 0
   self.appear_process = 0
   self.caster_channal = self.data.channal
   -- init skill
   self.attack_skill_vo = self.config.normal_attack
   self.skills = self.config.skills
   self.skills_coold = {}
  
   for i,v in ipairs(self.skills) do
        if v.skill_type == "passive" then
            local database = pack_data.pack_database(self,self,self.transform.grid_pos)
            local buff = buff.new(v,database)
            buff:execute(self.sess,self)
        else
            table.insert(self.skills_coold,{coold =v.coold,value = 0})
        end
   end


   self.hp = self.property:get("hp")
   self.max_hp = self.hp
end



function this:update( delta )
    self.delta = delta
    
    self.super:update(delta)

    self.betree:update(delta)

    self.transform:update(delta)

    self.buffcont:update(delta)

    self:update_coold(delta)

    if self.live_time > 0 then
        self.live_time = self.live_time - delta
        if self.live_time <= 0 then
            self.hp = 0
            self:die()
        end
    end
end

function this:update_coold(delta)
    local factor =1/ (1-self.property:get("coold_reduce"))
    for _,v in ipairs(self.skills_coold) do
        v.value = math.max( 0,v.value - delta*factor )
    end
end

function this:dispatch( name,src )
    if self.alive > 0 and name ~= "on_die" and name ~= "on_undying" and name ~= "post_die" then 
        return 
    end
    local trigger = self.sess.trigger
    self.buffcont:handle(self.sess, name)
    --trigger:handle_ev(name, self)
end

function this:do_attack( delta ,enemy)
    local old_value = self.attack_process
    self.attack_process = self.attack_process + delta
    local trace_data = trace.trace_attack(self,enemy)
    self.sess.trace:push(trace_data)
    if old_value < 0.45 and self.attack_process >= 0.45 then
        local database = pack_data.pack_database(self,enemy,self.transform.grid_pos)
        local attack_skill = entire_skill.new(self.sess,self.attack_skill_vo)
        self:on_attack() 
        attack_skill:execute(database)
    end
    if self.attack_process >= 1 then
        self.attack_process = 0
        return true
    end
    return false
end

function this:do_skill(delta,target,pos ,index )
    local old_value = self.skill_process
    self.skill_process = self.skill_process + delta
    if old_value <self.caster_channal and self.skill_process >= self.caster_channal then
        local database = pack_data.pack_database(self,target,pos)
        local skill = entire_skill.new(self.sess,self.skills[index])
        self.skills_coold[index].value = self.skills_coold[index].coold
        skill:execute(database)
    end
    if self.skill_process >= self.caster_channal*2 then
        self.skill_process = 0
        return true
    end
    return false
end

function this:do_appear( delta )
    self.appear_process = self.appear_process +delta
    if self.appear_process >= self.data.ready_time then
        self.appeared = 1
        return true
    end
    return false
end

function this:damage( value,source )
    self.hp = self.hp - value
    if self.threat_value[source.uid] == nil then
        self.threat_value[source.uid] = 10
    end
    self.threat_value[source.uid] = self.threat_value[source.uid] + 1
    self.entity:SetHp(self.hp,self.max_hp,0)
    if self.hp <= 0 then
        self.hp = 0
        source:on_kill()
        self:die()
    end
end

function this:heal(value,source  )
    self.hp = self.hp + value
    self.hp = math.min( self.max_hp,self.hp)
    self.entity:SetHp(self.hp,self.max_hp,1)
end

function this:die(  )
    if self.alive ~= 0 then return end
    self.alive = 2
    self:on_die()
    self.sess.field:unit_die(self)
    self.entity:Die(self.card_uid)
end




return this