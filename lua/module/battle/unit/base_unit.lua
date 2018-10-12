--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-05 17:36:20
]]
local this = class("base_unit")
local property = require("module.battle.unit.component.property")

function this:ctor( prop )
    self.name = prop.name
    self.property = property.new(self,prop)
end

function this:update(  )
    -- body
end

return this