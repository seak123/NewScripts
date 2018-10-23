--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-22 22:13:50
]]
local this = class("ai_sys")
local ai_config = require("module.battle.unit.behavior_tree.test_ai")

function this:ctor(  )
    self.ai_root= ai_config
end

function this:update(  )
    self.ai_root.update()
end

return this