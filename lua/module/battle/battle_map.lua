
local this = class("battle_map")
local battle_def = require("module.battle.battle_def")

function this:ctor( sess )
    self.room_table = {
        [33] = {X=470,Y=470},
        [32] = {X=470,Y=290},
        [31] = {X=470,Y=110},
        [23] = {X=290,Y=470},
        [22] = {X=290,Y=290},
        [21] = {X=290,Y=110},
        [13] = {X=110,Y=470},
        [12] = {X=110,Y=290},
        [11] = {X=110,Y=110}

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