
local this = class("battle_map")
local battle_def = require("module.battle.battle_def")

function this:ctor( sess )
    -- self.room_table = {
    --     [33] = {X=500,Y=500},
    --     [32] = {X=500,Y=310},
    --     [31] = {X=500,Y=120},
    --     [23] = {X=310,Y=500},
    --     [22] = {X=310,Y=310},
    --     [21] = {X=310,Y=120},
    --     [13] = {X=120,Y=500},
    --     [12] = {X=120,Y=310},
    --     [11] = {X=120,Y=120}

    -- }
    self.room_table = {}
    self.sess = sess
    self:init()
end

function this:init(  )

    -------------- init data
    local room_row = self.sess.vo.roomRow
    local start_row = 3
    local room_col = self.sess.vo.roomCol
    local start_col = 3 - math.floor(room_col/2)
    for i=start_row,start_row+room_row-1 do
        for j=start_col,start_col+room_col-1 do
            self.room_table[i*10+j] = {
                X = (i-1)*(battle_def.room_bound + battle_def.room_interval) + battle_def.room_bound/2 + battle_def.room_interval,
                Y = (j-1)*(battle_def.room_bound + battle_def.room_interval) + battle_def.room_bound/2 + battle_def.room_interval,
            }
        end
    end
    
    self.entry_room = (start_row + room_row - 1)*10 + 3
    self.portal_room = self.entry_room + 10
    self.boss_room = self:get_boss_room()

    -- portal room
    self.room_table[-1] = {X=self:get_room_center(self.entry_room).X + battle_def.room_bound/2 + 75,Y=self:get_room_center(self.entry_room).Y}
    self.room_table[self.boss_room] = {X=self:get_room_center(self.boss_room+10).X-battle_def.room_interval-battle_def.room_bound*1.15,Y=self:get_room_center(self.boss_room+10).Y}
    -------------- init view
    ------create portal
    self.sess.map:CreatePortal(self.room_table[-1].X-25,self.room_table[-1].Y)
    ------ create room
    for id,pos in pairs(self.room_table) do
        if id == -1 then
        elseif id == self.boss_room then
            self.sess.map:CreateBossRoom(pos.X,pos.Y)
        else 
            self.sess.map:CreateRoom(pos.X,pos.Y)
        end
    end

    -- create wall
    for id,pos in pairs(self.room_table) do
        if id ~= -1 and id ~= self.boss_room then
        local tem_id
        tem_id = id + 10
        if self:get_room_center(tem_id) == nil and tem_id ~= self.portal_room and tem_id ~= self.boss_room then
            self.sess.map:CreateWall(pos.X + battle_def.room_bound/2+3,pos.Y+4,0)
        end
        tem_id = id - 10
        if self:get_room_center(tem_id) == nil and tem_id ~= self.portal_room and tem_id ~= self.boss_room then
            self.sess.map:CreateWall(pos.X - battle_def.room_bound/2-2,pos.Y+4,0)
        end
        tem_id = id + 1
        if self:get_room_center(tem_id) == nil and tem_id ~= self.portal_room and tem_id ~= self.boss_room then
            self.sess.map:CreateWall(pos.X ,pos.Y+ battle_def.room_bound/2+2,90)
        end
        tem_id = id - 1
        if self:get_room_center(tem_id) == nil and tem_id ~= self.portal_room and tem_id ~= self.boss_room then
            self.sess.map:CreateWall(pos.X,pos.Y- battle_def.room_bound/2-5,90)
        end
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
        if id ~= -1 and id ~= self.boss_room then
        local tem_id
        tem_id = id + 10
        if self:get_room_center(tem_id) ~= nil and tem_id ~= self.portal_room and tem_id ~= self.boss_room then
            local path = {}
            path.X = (self:get_room_center(tem_id).X + pos.X)/2
            path.Y = (self:get_room_center(tem_id).Y + pos.Y)/2
            path.R = 0
            path.Offset = {X=0,Y=0}
            check_path(path)
        end
        tem_id = id - 10
        if self:get_room_center(tem_id) ~= nil and tem_id ~= self.portal_room and tem_id ~= self.boss_room then
            local path = {}
            path.X = (self:get_room_center(tem_id).X + pos.X)/2
            path.Y = (self:get_room_center(tem_id).Y + pos.Y)/2
            path.R = 0
            path.Offset = {X=0,Y=0}
            check_path(path)
        end
        tem_id = id + 1
        if self:get_room_center(tem_id) ~= nil and tem_id ~= self.portal_room and tem_id ~= self.boss_room then
            local path = {}
            path.X = (self:get_room_center(tem_id).X + pos.X)/2
            path.Y = (self:get_room_center(tem_id).Y + pos.Y)/2
            path.R = 90
            path.Offset = {X=-2,Y=0}
            check_path(path)
        end
        tem_id = id - 1
        if self:get_room_center(tem_id) ~= nil and tem_id ~= self.portal_room and tem_id ~= self.boss_room then
            local path = {}
            path.X = (self:get_room_center(tem_id).X + pos.X)/2
            path.Y = (self:get_room_center(tem_id).Y + pos.Y)/2
            path.R = 90
            path.Offset = {X=-2,Y=0}
            check_path(path)
        end
    end
    end
    -- add boos path
    local boss_path = {
        X = self:get_room_center(self.boss_room).X + battle_def.room_bound*0.65 + battle_def.room_interval/2,
        Y = self:get_room_center(self.boss_room).Y,
        R = 0,
        Offset = {X = 0,Y = 0}
    }
    check_path(boss_path)

    for _,pos in ipairs(path_pos) do
        self.sess.map:CreatePath(pos.X+pos.Offset.X,pos.Y+pos.Offset.Y,pos.R)
    end
end

function this:get_room_center( id )
    return self.room_table[id]
end

function this:get_entry_room(  )
    -- temp
    return self.entry_room
end

function this:get_boss_room(  )
    return 23
end

return this