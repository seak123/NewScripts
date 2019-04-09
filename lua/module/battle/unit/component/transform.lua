
local this = class("transform")
local def = require("module.battle.battle_def")

this.AnimationState = {
    Idle = "Idle",
    Walk = "Walk",
    Caster = "Caster",
    Appear = "Appear"
}

function this:ctor( master,data )
    self.master = master
    local grid_X =  data.init_x
    local grid_Y = data.init_y
    self.grid_pos = {X = grid_X, Y = grid_Y}
    self.offset = 0
    self.des_pos = nil
    self.des_room = data.init_room
end

function this:change_room( next_room )
    self.master:on_leave_room()
    self.master.last_location = self.master.location
    self.master.location = next_room
    table.insert( self.master.arrived_rooms,next_room)
    self.master:on_enter_room()
end

function this:update( delta )
    if self.des_pos ~= nil then
        local speed = self.master.property:get("speed")
        if speed < def.MinSpeed then speed = def.MinSpeed end
        local value = delta * speed + self.offset
        self.grid_pos.X,self.grid_pos.Y,self.offset= self.master.entity:Move(self.des_pos.X,self.des_pos.Y,speed,value,nil,nil,nil)
        --self.master.entity:SetRotation(self.des_pos.X,self.des_pos.Y)
        self.des_pos = nil
    end
    
end


return this