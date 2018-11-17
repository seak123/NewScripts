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
    if self.vo.cantain_curr_target == false then table.insert( self.database.target_trace,self.targets[1].uid) end
    self.targets = {}
    self[self.vo.target_type.."_select"](self,sess)
  
    return "completed"
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

function this:check_repeat(  )
    return function ( unit )
        for _,u in ipairs(self.database.target_trace) do
            if u == unit.uid then return false end
        end
        return true
    end
end

return this