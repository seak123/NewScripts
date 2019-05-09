
local base = require("module.battle.unit.base_unit")
local this = class("creature",base)

local state_ctrl = require("module.battle.unit.component.state_control")
local transform = require("module.battle.unit.component.transform")
local property = require("module.battle.unit.component.property")
local behavior_tree = require("module.battle.unit.behavior_tree.behavior_tree")

function this:ctor( sess,data)
    self.sess = sess
    self.data = data
    self.id = data.id
    -- type: 0,creature;1,structure;-1,boss
    self.type = data.type
    -- genus: 1,ground 2,fly
    self.genus = data.genus

    self.uid = data.uid
    if self.type == 0 then
        self.ai_vo = require("config.ai_config.strategy_ai")
    else
        self.ai_vo = require("config.ai_config.strategy_structure_ai")   
    end
   

   self.delta = 0
   self.idle_time = 0

   self.name = data.name
   
   self.side = data.side
    
    self:init()
end

function this:init(  )
    -- wait for appear action
    self.appeared = 0
    --if self.data.init_room ~= -1 then
    self.location = self.data.init_room
    local init_pos = self.sess.battle_map:get_room_center(self.data.init_room)
    self.data.init_x = init_pos.X
    self.data.init_y = init_pos.Y
    
    local data = self.data
    if self.type == 0 or self.type == -1 then
        self.entity = self.sess.map:CreateEntity(self.data.id,self.uid,self.side,self.data.init_x,self.data.init_y,-1)
    else
        self.entity = self.sess.map:CreateEntity(self.data.id,self.uid,self.side,self.data.init_x,self.data.init_y,-1)
    end

   self.property = property.new(self,property.unpack_prop(data))
   self.statectrl = state_ctrl.new(self)
   self.transform = transform.new(self,data)
   self.betree = behavior_tree:build(self,self.ai_vo)
    
   self.appear_process = 0
end



function this:update( delta )
    self.delta = delta

    self.betree:update(delta)

    self.transform:update(delta)

end

function this:remove(  )
    self.entity:Remove()
end

function this:do_appear( delta )
    self.appear_process = self.appear_process +delta
    if self.appear_process >= self.data.ready_time then
        self.appeared = 1
        return true
    end
    return false
end

function this:do_disappear(  )
    
end

function this:leave_fight(  )
    
end

return this