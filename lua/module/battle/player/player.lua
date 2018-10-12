--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-05 17:43:03
]]
local this = class("player")

function this:ctor( vo )
    self.side = vo.side

    self.cards = {}
    self.ap = 0
    self.max_ap = 100
end