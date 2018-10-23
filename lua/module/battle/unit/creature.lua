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

this.move_action1 = {
    state = "running",
    des_pos = {X = 300,Y = 0},
    update = function(unit)
        unit.transform.des_pos = {X = 300,Y = 0}
     end
     ,
     check = function ( unit )
         if unit.transform.grid_pos.X == 300 and unit.transform.grid_pos.Y == 0 then
            this.move_action1.state = "completed"
            print("action1 completed")
         end
     end
}

this.move_action2 = {
    state = "running",
    des_pos = {X = 300,Y = 0},
    update = function(unit)
        unit.transform.des_pos = {X = 300,Y = 0}
     end
     ,
     check = function ( unit )
         if unit.transform.grid_pos.X == 300 and unit.transform.grid_pos.Y == 0 then
            this.move_action2.state = "completed"
            print("action2 completed")
         end
     end
}

function this:init(  )
    -- init event
    local function make_event(self,name)
        self[name] = function(obj, src) 
          obj:dispatch(name, src)
        end
    end
    -- event end
    print("@@create entity, X="..self.data.init_x.." Y="..self.data.init_y)
    self.entity = self.sess.map:CreateEntity(self.data.id,self.data.init_x,self.data.init_y)
    
    if self.id == 1 then
        self.action = this.move_action1
    else
        self.action = this.move_action2
    end
end



function this:update( delta )
    self.super:update(delta)

    if self.action.state == "running" then
        self.action.update(self)
        self.action.check(self)
    end

    self.transform:update(delta)
end




return this