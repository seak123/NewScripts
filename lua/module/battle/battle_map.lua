
local this = class("battle_map")
local battle_def = require("module.battle.battle_def")

function this:ctor( sess )
    self.room_table = {
        [33] = {X=410,Y=410},
        [32] = {X=410,Y=250},
        [31] = {X=410,Y=90},
        [23] = {X=250,Y=410},
        [22] = {X=250,Y=250},
        [21] = {X=250,Y=90},
        [13] = {X=90,Y=410},
        [12] = {X=90,Y=250},
        [11] = {X=90,Y=90}

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

function this:get_entry_room(  )
    -- temp
    return 32
end

return this