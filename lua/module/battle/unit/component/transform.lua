--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-06 14:23:05
]]
local this = class("transform")

function this:ctor( master,data )
    self.master = master
    self.speed = master.property:get("speed")
    self.position = {X= data.init_x,Y= data.init_y}
    self.destination = {X = data.init_x, Y = data.init_x}
end

function this:GetNextPostion( delta )
    local value = delta * self.speed
    self.master.sess.map:GetNextPostion(self.id,self.position.X,self.position.Y,self.destination.X,self.destination.Y)
end

function this:update( delta )
    self.master.entity:SetTransform(self.position.X,self.position.Y)
end

return this