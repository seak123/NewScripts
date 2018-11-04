--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-06 14:23:05
]]
local this = class("transform")

this.AnimationState = {
    Idle = "Idle",
    Walk = "Walk",
    Caster = "Caster"
}

function this:ctor( master,data )
    self.master = master
    local grid_X = data.init_x
    local grid_Y = data.init_y
    self.grid_pos = {X = grid_X, Y = grid_Y}
    self.offset = 0
    self.des_pos = nil
end


function this:update( delta )
    if self.des_pos ~= nil then
        local value = delta * self.master.property:get("speed") + self.offset
        self.grid_pos.X,self.grid_pos.Y,self.offset= self.master.entity:Move(self.des_pos.X,self.des_pos.Y,value,nil,nil,nil)
        self.master.entity:SetRotation(self.des_pos.X,self.des_pos.Y)
        self.des_pos = nil
    end
end

return this