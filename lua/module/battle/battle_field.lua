--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-05 12:38:12
]]
local this = class("battle_field")
local creature = require("module.battle.unit.creature")
local battle_def = require("module.battle.battle_def")

function this:ctor(sess )
    self.sess = sess
    self.units = {{},{}}
    self.counter = 0
end

function this:add_unit( data)
    print("create new unit "..data.name)
    local uid = self.counter
    local unit = creature.new(self.sess,data,uid)
    self.counter = self.counter + 1
    table.insert( self.units[data.side],unit)
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

function this:find_enemy( unit )
    local enemy_side = 3 - unit.side
    local enemy = nil
    local max_threat = -999
    for _,u in ipairs(self.units[enemy_side]) do
        if self:distance(unit,u) < battle_def.MAPMATRIX.row/2 then
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
    local min_dis = 999
    for _,u in ipairs(self.units[enemy_side]) do
        local threat = unit.threat_value[u.uid]
        local dis = self:distance(unit,u)
        if threat == max_threat and dis < min_dis then
            min_dis = dis
            enemy = u
        end
    end
    return enemy
end

function this:find_friend( unit,condition_func )
    local side = unit.side
    local min_dis = 999
    local friend = nil
    if condition_func == nil then
        for _,u in ipairs(self.units[side]) do
            local dis = self:distance(unit,u)
            if dis < min_dis and unit.uid ~= u.uid then
                min_dis = dis
                friend = u
            end
        end
    else
        for _,u in ipairs(self.units[side]) do
            local dis = self:distance(unit,u)
            if dis < min_dis and condition_func(unit,u) then
                min_dis = dis
                friend = u
            end
        end
    end
    return friend
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