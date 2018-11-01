
local base = require("module.battle.unit.base_unit")
local this = class("creature",base)

local transform = require("module.battle.unit.component.transform")
local property = require("module.battle.unit.component.property")
local behavior_tree = require("module.battle.unit.behavior_tree.behavior_tree")
local entire_skill = require("module.battle.skill.entire_skill")
local pack_data = require("module.battle.skill.utils.pack_database")
local config_mng = require("config.config_manager")

function this:ctor( sess,data )
    self.sess = sess
    self.id = data.id
    self.config = require(config_mng.get_config_path(self.id))
    self.name = data.name
    self.data = data
    self.property = property.new(self,property.unpack_prop(data))
    self.transform = transform.new(self,data)
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
 
    self.entity = self.sess.map:CreateEntity(self.data.id,self.data.init_x,self.data.init_y)
    
   -- attack cache
   self.attack_process = 0
   -- init skill
   self.attack_skill = entire_skill.new(self.sess,self.config.normal_attack)

   self.hp = self.property:get("hp")

   self.time = 0
end



function this:update( delta )
    self.super:update(delta)

    self.betree:update(delta)

    self.transform:update(delta)
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
        self.attack_skill:execute(database)
    end
    if self.attack_process >= 1 then
        self.attack_process = 0
        return true
    end
    return false
end

function this:damage( value,source )
    self.hp = self.hp - value
    if self.hp <= 0 then
        self.hp = 0
        source:on_kill()
        self:die()
    end
end

function this:heal(value,source  )
    self.hp = self.hp + value
end

function this:die(  )
    self.alive = 2
    self:on_die()
    self.sess.field:unit_die(self)
    self.entity:Die()
end




return this