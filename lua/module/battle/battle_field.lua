
local this = class("battle_field")
local creature = require("module.battle.unit.creature")
local battle_def = require("module.battle.battle_def")
local bit = require("utils.bit_calc")

function this:ctor(sess )
    self.sess = sess
    self.units = {{},{}}
    self.room_units = {}
    self.counter = 0

    -- init 
    for id,_ in pairs(sess.battle_map.room_table) do
        self.room_units[id] = {}
    end
end

function this:add_unit( data,struct_uid)
    local uid = self.counter

    print("create new unit "..data.name.." uid:"..uid)

    local unit = creature.new(self.sess,data,uid,struct_uid)
    self.counter = self.counter + 1
    table.insert( self.units[data.side],unit)
    table.insert(self.room_units[data.init_room],unit)
    return unit
end

function this:unit_die( unit )
        local side = unit.side
        local location = unit.location
        for i, v in ipairs(self.units[side]) do
            if v.uid == unit.uid then
                table.remove( self.units[side], i )
                break
            end
        end
        for i, v in ipairs(self.room_units[location]) do
            if v.uid == unit.uid then
                table.remove( self.room_units[location], i )
                return
            end
        end
end

function this:change_room( unit,new_room )
    local location = unit.location
    unit.transform:change_room(new_room)
    for i, v in ipairs(self.room_units[location]) do
        if v.uid == unit.uid then
            table.remove( self.room_units[location], i )
            break
        end
    end
    table.insert( self.room_units[new_room], unit )
end

function this:portal( unit,new_room )
    -- local location = unit.location
    -- unit.transform:portal(new_room,out_door)
    -- for i, v in ipairs(self.room_units[location]) do
    --     if v.uid == unit.uid then
    --         table.remove( self.room_units[location], i )
    --         break
    --     end
    -- end
    -- table.insert( self.room_units[new_room], unit )
end

function this:get_unit( uid,side)
    if side ~= nil then
        for _, v in ipairs(self.units[side]) do
            if v.uid == uid then
                return v
            end
        end
    else
        for i=1,2 do
            for _, v in ipairs(self.units[i]) do
                if v.uid == uid then
                    return v
                end
            end
        end
    end
    return nil
end
--opposite_type: attack ground or sky
--just for normal attack
-- range: 0,all map  1,only room
-- active: 0,not active 1,active
function this:find_enemy( with_structure,unit,is_find_friend,active,range)
    local enemy_side = 3 - unit.side
    if is_find_friend ~= nil and is_find_friend == true then
        enemy_side = unit.side
    end
    local enemy = nil
    local threat_enemy = nil
    local max_threat = -9999
    local min_dis = 9999

    local type_flag = 0
    if with_structure == true then type_flag = 2 else type_flag =1 end

    local units = nil
    if range == nil or range == 1 then
        units = self.room_units[unit.location]
    else
        units = self.units[enemy_side]
    end

    for _,u in ipairs(units) do
        local dis = self:distance(unit,u)
        if  u.type < type_flag and u.side == enemy_side then
            local threat = unit.threat_value[u.uid]
            if threat ~= nil or active == 1 then
                if threat == nil then
                    -- set base threat_value
                    unit.threat_value[u.uid] = 10
                    threat = 10
                end

                if dis < min_dis then
                    min_dis = dis
                    enemy = u
                end
            end
        end
    end

    return enemy
end

function this:get_units(with_structure,side,unit,num,condition_func,range  )
    local opposite_type = unit.opposite_type
    local min_dis = 9999
    local enemy = {}
    for i=1,num do
        enemy[i] = {unit = nil,dis = 999}
    end

    local type_flag = 0
    if with_structure == true then type_flag = 2 else type_flag = 1 end
    if condition_func == nil then condition_func = function(a) return true end end

    local units = nil
    if range == nil or range == 1 then
        units = self.room_units[unit.location]
    else
        units = self.units[side]
    end

    for _,u in ipairs(units) do
        if condition_func(u) and u.type < type_flag and u.side == side  then
            local dis = self:distance(unit,u)
            local index = -1
            for i=num,1,-1 do
                if dis < enemy[i].dis then
                    index = i
                else
                    break
                end
            end
            if index ~= -1 then
                for i= num,index+1,-1 do
                    enemy[i].unit = enemy[i-1].unit
                    enemy[i].dis = enemy[i-1].dis
                end
                enemy[index].unit = u
                enemy[index].dis = dis
            end
        end
    end
    local res = {}
    for i=1,num do
        if enemy[i].unit ~= nil then table.insert( res, enemy[i].unit ) end
    end
    return res
end

function this:find_friend( with_structure,unit,condition_func ,range)
    local side = unit.side
    local min_dis = 9999
    local friend = nil

    local type_flag = 0
    if with_structure == true then type_flag = 2 else type_flag =1 end

    local units = nil
    if range == nil or range == 1 then
        units = self.room_units[unit.location]
    else
        units = self.units[side]
    end
    if condition_func == nil then
        for _,u in ipairs(units) do
            local dis = self:distance(unit,u)
            if dis < min_dis and unit.uid ~= u.uid and u.type < type_flag and u.side == side then
                min_dis = dis
                friend = u
            end
        end
    else
        for _,u in ipairs(units) do
            local dis = self:distance(unit,u)
            if dis < min_dis and condition_func(u) and u.type < type_flag and u.side == side then
                min_dis = dis
                friend = u
            end
        end
    end
    return friend
end

function this:find_random_unit(with_structure,side,unit,condition_func,range )

    local type_flag = 0
    if with_structure == true then type_flag = 2 else type_flag =1 end
    if condition_func == nil then condition_func = function(a) return true end end

    local res = {}

    local units = nil
    if range == nil or range == 1 then
        units = self.room_units[unit.location]
    else
        units = self.units[side]
    end

    for _,u in ipairs(units) do
       if u.type < type_flag and condition_func(u) then table.insert( res, u ) end
    end
    if #res ==0 then return nil end
    local index = math.random( 1, #res )
    return res[index]
end

function this:get_targets(opposite_type,with_structure,side,unit,num,condition_func,range)
    local min_dis = 9999
    local enemy = {}
    if num == -1 then num = 99 end
    for i=1,num do
        enemy[i] = {unit = nil,dis = 999}
    end

    local type_flag = 0
    if with_structure == true then type_flag = 2 else type_flag = 1 end
    if condition_func == nil then condition_func = function(a) return true end end

    local units = nil
    if range == nil or range == 1 then
        units = self.room_units[unit.location]
    else
        units = self.units[side]
    end
    for _,u in ipairs(units) do
        if condition_func(u) and u.type < type_flag and u.side == side then
            local dis = self:distance(unit,u)
            local index = -1
            for i=num,1,-1 do
                if dis < enemy[i].dis then
                    index = i
                else
                    break
                end
            end
            if index ~= -1 then
                for i= num,index+1,-1 do
                    enemy[i].unit = enemy[i-1].unit
                    enemy[i].dis = enemy[i-1].dis
                end
                enemy[index].unit = u
                enemy[index].dis = dis
            end
        end
    end
    local res = {}
    for i=1,num do
        if enemy[i].unit ~= nil then table.insert( res, enemy[i].unit ) end
    end
    return res
end

-- args can be unit or pos
function this:distance(a_unit,b_unit  )
    local a_pos = a_unit
    local b_pos = b_unit
    if a_unit.transform ~= nil then
        a_pos = {X = a_unit.transform.grid_pos.X,Y = a_unit.transform.grid_pos.Y}
    end
    if b_unit.transform ~= nil then
        b_pos = {X = b_unit.transform.grid_pos.X,Y = b_unit.transform.grid_pos.Y}
    end
    local x =a_pos.X - b_pos.X
    local y = a_pos.Y - b_pos.Y
    return math.sqrt( x*x + y*y )
end

function this:update( delta )
    
    for side,array in pairs(self.units) do
        for _,unit in ipairs(array) do
            unit:update(delta)
        end
    end
end

return this