
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

    --init data
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

function this:init(  )
    -- decorator init

     --sel init
     if self.controll_type == "sel" then
        self.priority_que = {}
        for _,v in ipairs(self.childs) do
            local index = v.vo.priority
            if self.priority_que[index] == nil then
                self.priority_que[index] = {}
            end
            table.insert( self.priority_que[index],v)
        end
    end
end

function this:update( delta )
    if self.running == false then
        for _,v in ipairs(self.decorators) do
            if v:check() == false then
                return "failure"
            end
        end
    end
    return self["update_by_"..self.controll_type](self,delta)
    
end

function this:abort(  )
    return self["abort_by_"..self.controll_type](self)
end

function this:update_by_sel( delta )
    if self.running == true then
        local run_priority = self.active_node.vo.priority
        if run_priority < #self.priority_que then
            for index = #self.priority_que,run_priority+1,-1 do
                local childs = self.priority_que[index]
                for _,n in ipairs(childs) do
                    local state = n:update(delta)
                    if state == "running" then
                        self:abort()
                        self.active_node = n
                        self.running = true
                        return "running"
                    end
                    if state == "completed" then
                        self:abort()
                        self.running = false
                        self.active_node = nil
                        return "completed"
                    end
                end
            end
        end
        local state = self.active_node:update(delta)
        if state == "running" then
            return "running"
        end
        if state == "completed" then
            self.running = false
            self.active_node = nil
            return "completed"
        end
        self.running = false
        self.active_node = nil
        return "failure"
    end
    for index=#self.priority_que,1,-1 do
        local childs = self.priority_que[index]
        for _,n in ipairs(childs) do
            local state = n:update(delta)
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
    end
    self.running = false
    self.active_node = nil
    return "failure"
end

function this:abort_by_sel(  )
    if self.running == true then
        self.running = false
        self.active_node:abort()
    end
end

function this:update_by_seq( delta )
    for i = self.active_index,#self.childs do
        local n = self.childs[i]
        local state = n:update(delta)
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

function this:abort_by_seq(  )
    if self.running == true then
        self.running = false
        self.active_node:abort()
    end
end

function this:update_by_par(  )
    
end


return this