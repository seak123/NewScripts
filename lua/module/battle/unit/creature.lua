
local base = require("module.battle.unit.base_unit")
local this = class("creature",base)

local transform = require("module.battle.unit.component.transform")
local state_ctrl = require("module.battle.unit.component.state_control")
local property = require("module.battle.unit.component.property")
local buffcont = require("module.battle.unit.component.buff_container")
local behavior_tree = require("module.battle.unit.behavior_tree.behavior_tree")
local entire_skill = require("module.battle.skill.entire_skill")
local pack_data = require("module.battle.skill.utils.pack_database")
local config_mng = require("config.config_manager")

function this:ctor( sess,data,uid )
    self.sess = sess
    self.id = data.id
    -- type: 1,creature;2,structure
    self.type = data.type
    self.uid = uid
    self.config = require(config_mng.get_unit_config(self.id))
    self.name = data.name
    self.data = data
    self.side = data.side
    self.property = property.new(self,property.unpack_prop(data))
    self.buffcont = buffcont.new(self)
    self.transform = transform.new(self,data)
    self.statectrl = state_ctrl.new(self)
    self.betree = behavior_tree:build(self,self.config.ai_vo)
    self:init()

    -- threat_value to every enemy: key is uid,  ps: value will not be cleared
    self.threat_value = {}
end

local function make_event(name)
    this[name] = function(obj, src) 
      obj:dispatch(name, src)
    end
end
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

function this:init(  )
    -- init data
    self.alive = 0
    -- wait for appear action
    self.appeared = 0
 
    self.entity = self.sess.map:CreateEntity(self.data.id,self.uid,self.side,self.data.init_x,self.data.init_y)
    
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
       self.skills_coold[i] = {coold =v.coold,value = 0}
   end

   self.hp = self.property:get("hp")
   self.max_hp = self.hp

   self.time = 0
end



function this:update( delta )
    self.super:update(delta)

    self.betree:update(delta)

    self.transform:update(delta)

    self.buffcont:update(delta)

    self:update_coold(delta)
end

function this:update_coold(delta)
    for _,v in ipairs(self.skills_coold) do
        v.value = math.max( 0,v.value - delta )
    end
end

function this:dispatch( name,src )
    -- if self.alive > 2 and name ~= "on_die" and name ~= "on_undying" and name ~= "post_die" then 
    --     return 
    -- end
    -- local systr = self.sess.systr
    -- self.buffcont:handle(self.sess, name)
    -- systr:handle_ev(name, self)
end

function this:do_attack( delta ,enemy)

    local old_value = self.attack_process
    self.attack_process = self.attack_process + delta
    if old_value < 0.5 and self.attack_process >= 0.5 then
        local database = pack_data.pack_database(self,enemy,self.transform.grid_pos)
        local attack_skill = entire_skill.new(self.sess,self.attack_skill_vo) 
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
    self.entity:SetHp(self.hp,self.max_hp)
    if self.hp <= 0 then
        self.hp = 0
        source:on_kill()
        self:die()
    end
end

function this:heal(value,source  )
    self.hp = self.hp + value
    self.hp = math.min( self.max_hp,self.hp )
    self.entity:SetHp(self.hp,self.max_hp)
end

function this:die(  )
    self.alive = 2
    self:on_die()
    self.sess.field:unit_die(self)
    self.entity:Die()
end




return this