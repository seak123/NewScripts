--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-05 12:38:12
]]
local this = class("battle_field")

function this:ctor( )
    self.units = {}
    self.counter = 0
end

function this:add_unit( unit )
    print("create new unit "..unit.name)
    unit.uid = self.counter
    self.counter = self.counter + 1
    table.insert( self.units,unit)
end

function this:update( delta )
    for _,unit in ipairs(self.units) do
        unit:update(delta)
    end
end

return this