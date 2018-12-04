
--local base = require("module.battle.unit.behavior_tree.base_node")
local this = class("ai_behavior")

function this:ctor( vo ,database)
    -- sel;par;seq;
    self.vo = vo
    self.controll_type = vo.controll_type
    self.childs = {}
    self.decorators = {}
   
    --seq args
    self.active_index = 1

    --init data
    self:init_data(database)
    if self.vo.decorators ~= nil then
        for _,v in ipairs(self.vo.decorators) do
            print("@@"..v.execute)
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

function this:execute( )
    for _,v in ipairs(self.decorators) do
        if v:check() == false then
            return false
        end
    end
    return self["enter_by_"..self.controll_type](self) 
end

function this:enter_by_sel( )
    for index=#self.priority_que,1,-1 do
        local childs = self.priority_que[index]
        for _,n in ipairs(childs) do
            local state = n:execute()
            if state == true then
                return true
            end
        end
    end
    return false
end

function this:enter_by_seq( )
    for i = self.active_index,#self.childs do
        local n = self.childs[i]
        local state = n:execute()
        if state == true then
            return true
        end
        if state == false then
            return false
        end
    end
end

function this:update_by_par(  )
    
end


return this