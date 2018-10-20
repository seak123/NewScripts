--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-05 17:36:20
]]
local this = class("base_unit")
local property = require("module.battle.unit.component.property")

function this:ctor( sess,data )
    
end

function this:update(  )
   
end



function this:init(  )
    -- init event
    local function make_event(self,name)
        self[name] = function(obj, src) 
          obj:dispatch(name, src)
        end
    end
    -- event end
    self.entity = self.sess.map:CreateEntity(self.data.id,self.data.init_x,self.data.init_y)

end

  

return this