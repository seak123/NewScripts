--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-06 14:23:05
]]
local this = class("transform")

function this:ctor( master,data )
    self.master = master
    self.position = {X= data.init_x,Y= data.init_y}
    local grid_X = 0
    local grid_Y = 0
    grid_X,grid_Y = self.master.sess.map:GetGridPos(self.position.X,self.position.Y,nil,nil)
    self.grid_pos = {X = grid_X, Y = grid_Y}
    self.offset = 0
    self.des_pos = nil
end


function this:update( delta,des_pos )
    self.des_pos = des_pos
    local can_move = false
    local completed = false
    if self.des_pos ~= nil then
        local value = delta * self.master.property:get("speed") + self.offset
        can_move,self.grid_pos.X,self.grid_pos.Y,self.offset,completed = self.master.sess.map:TryMove(self.master.id,self.grid_pos.X,self.grid_pos.Y,self.des_pos.X,self.des_pos.Y,value,nil,nil,nil,nil,nil)
    end
    if can_move == true then
        self.position.X,self.position.Y = self.master.sess.map:GetLogicPos(self.grid_pos.X,self.grid_pos.Y,nil,nil)
        --print("id:"..self.master.id.." new pos:"..self.position.X.." "..self.position.Y)
        self.master.entity:SetTransform(self.position.X,self.position.Y)
    end
    self.des_pos = nil
    return completed
end

return this