--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-23 22:52:52
]]
--local base = require("module.battle.unit.behavior_tree.base_node")
local this = class("behavior_node")

function this:ctor( vo ,database)
    -- sel;par;seq;
    self.vo = vo
    self.controll_type = vo.controll_type
    self.childs = {}
    self.decorators = {}
    self.running = false
    self.active_node = nil
    --seq args
    self.active_index = 1

    --init
    self:init_data(database)
    if self.vo.decorators ~= nil then
        for _,v in ipairs(self.vo.decorators) do
            local dec = require(v.execute).new(v)
            dec:init_data(database)
            table.insert( self.decorators, dec )
        end
    end
end

function this:init_data( database )
    self.database = database
end

function this:update(  )
    for _,v in ipairs(self.decorators) do
        print("@@check")
        if v:check() == false then
            print("@@check failure")
            return "failure"
        end
    end
    return self["update_by_"..self.controll_type](self)
    
end

function this:update_by_sel(  )
    if self.running == true then
        local state = self.active_node:update()
        if state == "running" then
            return "running"
        end
        if state == "completed" then
            self.running = false
            self.active_node = nil
            return "completed"
        end
    end
    for _,n in ipairs(self.childs) do
        local state = n:update()
        if state == "running" then
            self.active_node = n
            self.running = true
           return "running"
        end
        if state == "completed" then
            self.running = false
            self.active_node = nil
            return "completed"
        end
    end
    self.running = false
    self.active_node = nil
    return "failure"
end

function this:update_by_seq(  )
    for i = self.active_index,#self.childs do
        local n = self.childs[i]
        local state = n:update()
        if state == "running" then
            self.active_index = i
            self.running = true
            self.active_node = n
            return "running"
        end
        if state == "failure" then
            self.active_index = 1
            self.running = false
            self.active_node = nil
            return "failure"
        end
        self.active_index = i
        if self.active_index == #self.childs then
            self.active_index = 1
            self.running = false
            self.active_node = nil
            return "completed"
        end
    end
end

function this:update_by_par(  )
    
end


return this