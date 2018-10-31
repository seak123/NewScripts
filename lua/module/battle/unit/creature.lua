
local base = require("module.battle.unit.base_unit")
local this = class("creature",base)

local transform = require("module.battle.unit.component.transform")
local property = require("module.battle.unit.component.property")
local behavior_tree = require("module.battle.unit.behavior_tree.behavior_tree")
local entire_skill = require("module.battle.skill.entire_skill")
local bt_config = require("module.battle.unit.component.test_ai_config")
local attack_config = require("module.battle.unit.component.test_normal_attack")
local pack_data = require("module.battle.skill.utils.pack_database")

function this:ctor( sess,data )
    self.sess = sess
    self.id = data.id
    self.name = data.name
    self.data = data
    self.property = property.new(self,property.unpack_prop(data))
    self.transform = transform.new(self,data)
    self.betree = behavior_tree:build(self,bt_config)
    self:init()

    -- threat_value to every enemy: key is uid,  ps: value will not be cleared
    self.threat_value = {}
end

this.move_action1 = {
    state = "running",
    des_pos = {X = 300,Y = 0},
    update = function(unit)
        unit.transform.des_pos = {X = 300,Y = 0}
     end
     ,
     check = function ( unit )
         if unit.transform.grid_pos.X == 300 and unit.transform.grid_pos.Y == 0 then
            this.move_action1.state = "completed"
         end
     end
}

this.move_action2 = {
    state = "running",
    des_pos = {X = 300,Y = 0},
    update = function(unit)
        unit.transform.des_pos = {X = 300,Y = 0}
     end
     ,
     check = function ( unit )
         if unit.transform.grid_pos.X == 300 and unit.transform.grid_pos.Y == 0 then
            this.move_action2.state = "completed"
         end
     end
}

function this:init(  )
    -- init event
    local function make_event(self,name)
        self[name] = function(obj, src) 
          obj:dispatch(name, src)
        end
    end
    -- event end
    self.entity = self.sess.map:CreateEntity(self.data.id,self.data.init_x,self.data.init_y)
    
   -- attack cache
   self.attack_process = 0
   -- init skill
   self.attack_skill = entire_skill.new(self.sess,attack_config)

   self.hp = self.property:get("hp")

   self.time = 0
end



function this:update( delta )
    self.super:update(delta)

    self.betree:update(delta)

    self.transform:update(delta)
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
    print("get damage "..value)
end




return this