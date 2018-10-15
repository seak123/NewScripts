--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-06 13:10:52
]]
local base = require("module.battle.unit.base_unit")
local this = class("creature",base)

local transform = require("module.battle.unit.component.transform")

function this:ctor( sess,data )
    self.super:ctor(sess,data)
    self.transform = transform.new(self,data)
    self.init()
end

function this:init(  )
    this.super:init()
end

function this:update( delta )
    self.super:update(delta)
    local des_pos = {X=0,Y=0}
    self.transform:update(delta,des_pos)
end




return this