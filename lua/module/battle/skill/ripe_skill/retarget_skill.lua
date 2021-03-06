local base = require("module.battle.skill.ripe_skill.base_skill")
local this = class("retarget_skill",base)

function this:ctor( vo,database )
    self.database = database
    self.vo = vo
    self.targets = {}
    --self:build_to_array("childs",vo.childs,database)
end

function this:execute( sess,delta )
    if self.vo.target_source == 0 then
        self.target = self.targets[1]
    else
        self.target = sess.field:get_unit(sess.trace:get_last_data().target_uid)
    end
    if self.vo.is_friend == false then
        self.target_side = 3 - self.database.caster.side
        if self.vo.cantain_curr_target == false then table.insert( self.database.target_trace,self.target.uid) end
    else
        self.target_side = self.database.caster.side
        if self.vo.cantain_curr_target == false then table.insert( self.database.target_trace,self.target.uid) end
    end
    
    if self.vo.on_target == 0 then
        self.center_unit = self.database.caster
    else
        self.center_unit = self.target
    end
    
    self.targets = {}
    self[self.vo.target_type.."_select"](self,sess)
  
    return "completed"
end

function this:self_select( sess )
    table.insert( self.targets, self.center_unit)
    table.insert( self.database.target_trace, self.center_unit.uid )
end

function this:random_select( sess )
    local func
    if self.vo.can_repeat == true then
        func = nil
    else
        func = self:check_repeat()
    end
    for i=1,self.vo.num do
        local unit = sess.field:find_random_unit(false,self.target_side,self.center_unit,func)
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
        local unit = sess.field:get_units(false,self.target_side,self.center_unit,self.vo.num,func)
        --local close_unit = field:get_units(true,3-database.main_castle.side,database.main_castle,1)[1]
        if unit ~= nil then
            for _,u in ipairs(unit) do
                if sess.field:distance(u,self.center_unit) <= self.vo.distance then
                    table.insert( self.targets, u )
                    table.insert( self.database.target_trace, u.uid )
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
        local unit = sess.field:find_random_unit(false,self.target_side,self.center_unit,func)
        if unit == nil then
            break
        end
        table.insert( self.targets,unit)
        table.insert( self.database.target_trace,unit.uid )
    end
    for i=#self.targets+1,self.vo.num do
        local unit = sess.field:find_random_unit(false,self.target_side,self.center_unit,func2)
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