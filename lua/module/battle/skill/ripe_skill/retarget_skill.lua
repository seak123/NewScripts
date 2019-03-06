local base = require("module.battle.skill.ripe_skill.base_skill")
local this = class("retarget_skill",base)

function this:ctor( vo,database )
    self.database = database
    self.vo = vo
    self.targets = {}
    --self:build_to_array("childs",vo.childs,database)
end

function this:execute( sess,delta )
    self.target_side = self.targets[1].side
    if self.vo.cantain_curr_target == false then table.insert( self.database.target_trace,sess.trace:get_last_data().target) end
    self.targets = {}
    self[self.vo.target_type.."_select"](self,sess)
  
    return "completed"
end

function this:self_select( sess )
    table.insert( self.targets, self.database.master)
    table.insert( self.database.target_trace, self.database.master.uid )
end

function this:random_select( sess )
    local func
    if self.vo.can_repeat == true then
        func = nil
    else
        func = self:check_repeat()
    end
    for i=1,self.vo.num do
        local unit = sess.field:find_random_unit(false,self.target_side,func)
        if unit ~= nil then
            table.insert( self.targets, unit )
            table.insert( self.database.target_trace, unit.uid ) 
        end
    end
end

function this:distance_select( sess )
    local func
    if self.vo.can_repeat == true then
        func = nil
    else
        func = self:check_repeat()
    end
    --for i=1,self.vo.num do
        local unit = sess.field:get_units(false,self.target_side,self.database.caster,self.vo.num,func)
        --local close_unit = field:get_units(true,3-database.main_castle.side,database.main_castle,1)[1]
        if unit ~= nil then
            for _,u in ipairs(unit) do
                if sess.field:distance(u,self.database.caster) <= self.vo.distance then
                    table.insert( self.targets, u )
                    table.insert( self.database.target_trace, unit.uid )
                end
            end 
        end
    --end
end

function this:random_hurted_select( sess )
    local func,func2
    if self.vo.can_repeat == true then
        func = function(unit)
            return unit.hp<unit.max_hp
        end
        func2 = nil
    else
        func = function ( unit )
            return self:check_repeat()(unit) and unit.hp<unit.max_hp
        end
        func2 = self:check_repeat()
    end
    for i=1,self.vo.num do
        local unit = sess.field:find_random_unit(false,self.target_side,func)
        if unit == nil then
            break
        end
        table.insert( self.targets,unit)
        table.insert( self.database.target_trace,unit.uid )
    end
    for i=#self.targets+1,self.vo.num do
        local unit = sess.field:find_random_unit(false,self.target_side,func2)
        if unit ~= nil then
            table.insert( self.targets,unit )
            table.insert( self.database.target_trace,unit.uid )
        end
    end
end

function this:check_repeat(  )
    return function ( unit )
        for _,u in ipairs(self.database.target_trace) do
            if u == unit.uid then return false end
        end
        return true
    end
end

return this