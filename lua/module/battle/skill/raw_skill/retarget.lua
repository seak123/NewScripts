local base = require("module.battle.skill.raw_skill.base_rawskill")
local this = class("retarget",base)

function this:ctor( vo,database )
    self.vo = vo
    self.database = database
    self:init_build(vo)
end

function this:execute( sess,target )
    self.target_side = target.side
    self.target = target
    self.target_trace = {}
    
    if self.vo.cantain_curr_target== false then table.insert( self.target_trace, target.uid) end
    local targets = self[self.vo.target_type.."_select"](self,sess)
    
    for _,unit in ipairs(targets) do
        self:execute_subs(sess,unit)
    end
    
end

function this:self_select( sess )
    if self:check_repeat()(self.database.caster) == true then
        return {self.database.caster}
    else
        return {}
    end
end

function this:random_select( sess )
    local targets = {}
    local func = self:check_repeat()

    for i=1,self.vo.num do
        local unit = sess.field:find_random_unit(false,self.target_side,self.database.caster,func)
        if unit ~= nil then
            table.insert( targets, unit )
            table.insert( self.target_trace, unit.uid ) 
        end
    end
    return targets
end

function this:distance_select( sess )
    local func = self:check_repeat()
    local targets = {}
    --for i=1,self.vo.num do
        local unit = sess.field:get_units(false,self.target_side,self.database.caster,self.vo.num,func)
        --local close_unit = field:get_units(true,3-database.main_castle.side,database.main_castle,1)[1]
        if unit ~= nil then
            for _,u in ipairs(unit) do
                if sess.field:distance(u,self.database.caster) <= self.vo.distance then
                    table.insert( targets, u )
                    table.insert( self.target_trace, unit.uid )
                end
            end 
        end
    --end
    return targets
end

function this:random_hurted_select( sess )
    local targets = {}
    local func,func2
 
    func = function ( unit )
        return self:check_repeat()(unit) and unit.hp<unit.max_hp
    end
    func2 = self:check_repeat()

    for i=1,self.vo.num do
        local unit = sess.field:find_random_unit(false,self.target_side,self.database.caster,func)
        if unit == nil then
            break
        end
        table.insert( targets,unit)
        table.insert( self.target_trace,unit.uid )
    end
    -- if hurted num is not enough
    for i=#self.targets+1,self.vo.num do
        local unit = sess.field:find_random_unit(false,self.target_side,self.database.caster,func2)
        if unit ~= nil then
            table.insert( targets,unit )
            table.insert( self.target_trace,unit.uid )
        end
    end
    return targets
end

function this:check_repeat(  )
    return function ( unit )
        for _, v in ipairs(self.target_trace) do
            if unit.uid == v then return false end
        end
        return true
    end
end

return this