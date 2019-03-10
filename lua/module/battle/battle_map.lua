
local this = class("battle_map")
local battle_def = require("module.battle.battle_def")

function this:ctor( sess )
    self.room_table = {
        [11] = {X=400,Y=400},
        [12] = {X=400,Y=240},
        [13] = {X=400,Y=80},
        [21] = {X=240,Y=400},
        [22] = {X=240,Y=240},
        [23] = {X=240,Y=80},
        [31] = {X=80,Y=400},
        [32] = {X=80,Y=240},
        [33] = {X=80,Y=80}

    }
    self.sess = sess
    self:init()
end

function this:init(  )
    for _,pos in pairs(self.room_table) do
        self.sess.map:CreateRoom(pos.X,pos.Y)
    end
end

function this:get_room_center( id )
    return self.room_table[id]
end

return this