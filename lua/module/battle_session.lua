--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-05 17:30:41
]]
local this = class("battle_session")
local battle_field = require("module.battle.battle_field")

function this:ctor(  )
    self.field = battle_field.new()
end

function this:update( delta )
    self.field:update(delta)
end

return this