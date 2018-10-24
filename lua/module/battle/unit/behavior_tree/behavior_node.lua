--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-23 22:52:52
]]
local base = require("module.battle.unit.behavior_tree.base_node")
local this = class("behavior_node")

function this:ctor(  )
    -- sel;par;seq;
    self.controll_type = "sel"
end

function this:update(  )
    for _,v in ipairs(self.decorator) do
        if v:check() == false then
            return "failure"
        end
    end
    
end

function this:update_by_sel(  )
    
end


return this