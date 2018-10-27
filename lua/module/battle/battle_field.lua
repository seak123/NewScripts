--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-05 12:38:12
]]
local this = class("battle_field")
local creature = require("module.battle.unit.creature")

function this:ctor(sess )
    self.sess = sess
    self.units = {{},{}}
    self.counter = 0
end

function this:add_unit( data)
    print("create new unit "..data.name)
    local unit = creature.new(self.sess,data)
    unit.uid = self.counter
    unit.side = data.side
    self.counter = self.counter + 1
    table.insert( self.units[data.side],unit)
end

function this:find_enemy( unit )
    local enemy_side = 3 - unit.side
    for _,u in ipairs(self.units[enemy_side]) do
        if self:distance(unit,u) < 250 then
            return u
        end
    end
    return nil
end

function this:distance(a_unit,b_unit  )
    local a_tran = a_unit.transform
    local b_tran = b_unit.transform
    local x = a_tran.grid_pos.X - b_tran.grid_pos.X
    local y = a_tran.grid_pos.Y - b_tran.grid_pos.Y
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