--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-05 21:24:06
]]
local this = class("property")
local def = require("module.battle.battle_def")

function this:ctor( master,prop )
    self:init()
    self.master = master
    for n,v in ipairs(prop) do
        self[n.."base"] = v
    end
end

function this:init(  )
    for n,v in ipairs(def.PROPERTY) do
        if v == def.RATE then
            self[n.."base"] = 0
            self[n.."add"] = 0 
        else
            self[n.."base"] = 0
            self[n.."rate"] = 0
            self[n.."add"] = 0
        end
        self[n] = 0
    end
end

function this:change_prop( name,value )
    self[name] = self[name] + value
end

function this:get( name )
    if def.PROPERTY[name] == def.RATE then
        return self[name.."base"] + self[name.."add"]
    else
        return self[name.."base"]*(1+self[name.."rate"] + self[name.."base"])
    end 
end

return this