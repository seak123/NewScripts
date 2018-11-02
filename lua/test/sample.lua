--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-06 14:25:12
]]
require("utils.functions")

local this = class("sample")
local session = require("module.battle_session")
local creature = require("module.battle.unit.creature")



function this.init( root )
    root.session = session.new()
end

return this