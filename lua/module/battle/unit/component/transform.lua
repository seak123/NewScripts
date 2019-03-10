
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

function this:portal( next_room,out_door )
    self.master.location = next_room
    local center = self.master.sess.battle_map:get_room_center(next_room)
    local pos = {}
    if out_door == 1 then
        pos.X = center.X + def.room_bound/2 - 16
        pos.Y = center.Y
    elseif out_door == 2 then
        pos.X = center.X 
        pos.Y = center.Y- def.room_bound/2 + 16
    elseif out_door == 3 then
        pos.X = center.X - def.room_bound/2 + 16
        pos.Y = center.Y
    else
        pos.X = center.X 
        pos.Y = center.Y + def.room_bound/2 - 16
    end
    self.master.entity:Portal(pos.X,pos.Y)
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