
local this = class("battle_field")
local creature = require("module.battle.unit.creature")
local battle_def = require("module.battle.battle_def")

function this:ctor(sess )
    self.sess = sess
    self.units = {{},{}}
    self.counter = 0
end

function this:add_unit( data,struct_uid)
    local uid = self.counter

    print("create new unit "..data.name.." uid:"..uid)

    local unit = creature.new(self.sess,data,uid,struct_uid)
    self.counter = self.counter + 1
    table.insert( self.units[data.side],unit)
    return unit
end

function this:unit_die( unit )
    local side = unit.side
    for i, v in ipairs(self.units[side]) do
        if v.uid == unit.uid then
            table.remove( self.units[side], i )
            return
        end
    end
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

function this:find_enemy( with_structure,unit )
    local enemy_side = 3 - unit.side
    local enemy = nil
    local max_threat = -9999

    local type_flag = 0
    if with_structure == true then type_flag = 2 else type_flag =1 end

    for _,u in ipairs(self.units[enemy_side]) do
        if self:distance(unit,u) < battle_def.MAPMATRIX.row/2 and u.type < type_flag then
            local threat = unit.threat_value[u.uid]
            if threat == nil then
                -- set base threat_value
                unit.threat_value[u.uid] = 10
                threat = 10
            end
            if  threat > max_threat then
                max_threat = threat
                enemy = u
            end
        end
    end
    local min_dis = 9999
    for _,u in ipairs(self.units[enemy_side]) do
        local threat = unit.threat_value[u.uid]
        local dis = self:distance(unit,u)
        if threat == max_threat and dis < min_dis and u.type< type_flag then
            min_dis = dis
            enemy = u
        end
    end
    return enemy
end

function this:get_units(with_structure,side,unit,num,condition_func  )
    local min_dis = 9999
    local enemy = {}
    for i=1,num do
        enemy[i] = {unit = nil,dis = 999}
    end

    local type_flag = 0
    if with_structure == true then type_flag = 2 else type_flag = 1 end
    if condition_func == nil then condition_func = function(a) return true end end
    for _,u in ipairs(self.units[side]) do
        if condition_func(u) and u.type < type_flag then
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

function this:find_friend( with_structure,unit,condition_func )
    local side = unit.side
    local min_dis = 9999
    local friend = nil

    local type_flag = 0
    if with_structure == true then type_flag = 2 else type_flag =1 end
    if condition_func == nil then
        for _,u in ipairs(self.units[side]) do
            local dis = self:distance(unit,u)
            if dis < min_dis and unit.uid ~= u.uid and u.type < type_flag then
                min_dis = dis
                friend = u
            end
        end
    else
        for _,u in ipairs(self.units[side]) do
            local dis = self:distance(unit,u)
            if dis < min_dis and condition_func(u) and u.type < type_flag then
                min_dis = dis
                friend = u
            end
        end
    end
    return friend
end

function this:find_random_unit(with_structure,side,condition_func )

    local type_flag = 0
    if with_structure == true then type_flag = 2 else type_flag =1 end
    if condition_func == nil then condition_func = function(a) return true end end

    local units = {}
    for _,u in ipairs(self.units[side]) do
       if u.type < type_flag and condition_func(u) then table.insert( units, u ) end
    end
    if #units ==0 then return nil end
    local index = math.random( 1, #units )
    return units[index]
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