--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-05 12:38:12
]]
local this = class("battle_field")
local creature = require("module.battle.unit.creature")

function this:ctor( )
    self.units = {{},{}}
    self.counter = 0
end

function this:add_unit( data)
    print("create new unit "..data.name)
    local unit = creature.new(data)
    unit.uid = self.counter
    unit.side = data.side
    self.counter = self.counter + 1
    table.insert( self.units[data.side],unit)
end

function this:update( delta )
    for side,array in pairs(self.units) do
        for _,unit in ipairs(array) do
            unit:update(delta)
        end
    end
end

return this