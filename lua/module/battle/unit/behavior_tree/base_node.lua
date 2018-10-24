--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-23 22:54:46
]]
local this = class("base_node")

function this:append( name,... )
    local args = ...
    if self[name] == nil then
        self[name] = {}
    end
    for _,v in ipairs(args) do
        table.insert( self[name], v)
    end
end


return this