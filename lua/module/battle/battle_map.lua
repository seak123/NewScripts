
local this = class("battle_map")
local battle_def = require("module.battle.battle_def")

function this:ctor( sess )
    self.room_table = {
        [33] = {X=500,Y=500},
        [32] = {X=500,Y=310},
        [31] = {X=500,Y=120},
        [23] = {X=310,Y=500},
        [22] = {X=310,Y=310},
        [21] = {X=310,Y=120},
        [13] = {X=120,Y=500},
        [12] = {X=120,Y=310},
        [11] = {X=120,Y=120}

    }
    self.sess = sess
    self:init()
end

function this:init(  )
    for _,pos in pairs(self.room_table) do
        self.sess.map:CreateRoom(pos.X,pos.Y)
    end
    -- create wall
    for id,pos in pairs(self.room_table) do
        local tem_id
        tem_id = id + 10
        if self:get_room_center(tem_id) == nil then
            self.sess.map:CreateWall(pos.X + battle_def.room_bound/2+2,pos.Y+4,0)
        end
        tem_id = id - 10
        if self:get_room_center(tem_id) == nil then
            self.sess.map:CreateWall(pos.X - battle_def.room_bound/2-2,pos.Y+4,0)
        end
        tem_id = id + 1
        if self:get_room_center(tem_id) == nil then
            self.sess.map:CreateWall(pos.X ,pos.Y+ battle_def.room_bound/2+5,90)
        end
        tem_id = id - 1
        if self:get_room_center(tem_id) == nil then
            self.sess.map:CreateWall(pos.X,pos.Y- battle_def.room_bound/2,90)
        end
    end

    -- create path
    local path_pos = {}
    local function check_path( new_pos )
        for _,pos in ipairs(path_pos) do
            if new_pos.X == pos.X and new_pos.Y == pos.Y then
                return false
            end
        end
        table.insert( path_pos,new_pos)
        return true
    end
    for id,pos in pairs(self.room_table) do
        local tem_id
        tem_id = id + 10
        if self:get_room_center(tem_id) ~= nil then
            local path = {}
            path.X = (self:get_room_center(tem_id).X + pos.X)/2
            path.Y = (self:get_room_center(tem_id).Y + pos.Y)/2
            path.R = 0
            path.Offset = {X=0,Y=4}
            check_path(path)
        end
        tem_id = id - 10
        if self:get_room_center(tem_id) ~= nil then
            local path = {}
            path.X = (self:get_room_center(tem_id).X + pos.X)/2
            path.Y = (self:get_room_center(tem_id).Y + pos.Y)/2
            path.R = 0
            path.Offset = {X=0,Y=4}
            check_path(path)
        end
        tem_id = id + 1
        if self:get_room_center(tem_id) ~= nil then
            local path = {}
            path.X = (self:get_room_center(tem_id).X + pos.X)/2
            path.Y = (self:get_room_center(tem_id).Y + pos.Y)/2
            path.R = 90
            path.Offset = {X=-2,Y=4}
            check_path(path)
        end
        tem_id = id - 1
        if self:get_room_center(tem_id) ~= nil then
            local path = {}
            path.X = (self:get_room_center(tem_id).X + pos.X)/2
            path.Y = (self:get_room_center(tem_id).Y + pos.Y)/2
            path.R = 90
            path.Offset = {X=-2,Y=4}
            check_path(path)
        end
    end
    for _,pos in ipairs(path_pos) do
        self.sess.map:CreatePath(pos.X+pos.Offset.X,pos.Y+pos.Offset.Y,pos.R)
    end
end

function this:get_room_center( id )
    return self.room_table[id]
end

function this:get_entry_room(  )
    -- temp
    return 32
end

function this:get_boss_room(  )
    return 2
end

return this