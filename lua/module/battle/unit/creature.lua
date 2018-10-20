--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-06 13:10:52
]]
local base = require("module.battle.unit.base_unit")
local this = class("creature",base)

local transform = require("module.battle.unit.component.transform")
local property = require("module.battle.unit.component.property")

function this:ctor( sess,data )
    self.sess = sess
    self.id = data.id
    self.name = data.name
    self.data = data
    self.property = property.new(self,property.unpack_prop(data))
    self.transform = transform.new(self,data)
    self:init()
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

function this:update( delta )
    self.super:update(delta)
    local des_pos = {X=447,Y=0}
    self.transform:update(delta,des_pos)
end




return this