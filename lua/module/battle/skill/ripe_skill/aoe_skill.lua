local base = require("module.battle.skill.ripe_skill.base_skill")
local this = class("aoe_skill",base)
local aoe_vo = require("module.battle.skill.ripe_skill_vo.aoe_skill_vo")

function this:ctor( vo,database )
    self.vo = vo
    self.database = database
    self.targets = {}
    self:build_to_array("raw_skills",vo.raw_skills,database)
    --self:build_to_array("childs",vo.childs,database)
    self.inited = false
    self.timepass = 0
    self.process = 0
    self.target_pos = {}
end


function this:execute( sess,delta )
    if self.inited == false then
        if self.vo.target == aoe_vo.Target.Unit then
            if self.targets[1].alive ~= 0 then return "completed" end 
            self.target_pos.X = self.targets[1].transform.grid_pos.X
            self.target_pos.Y = self.targets[1].transform.grid_pos.Y
        elseif self.vo.target == aoe_vo.Target.Pos then
            self.target_pos.X = self.database.target_pos.X
            self.target_pos.Y = self.database.target_pos.Y
        end
        --target trace
        if self.vo.cantain_curr_target == false then table.insert( self.database.target_trace,self.targets[1].uid) end

        --side
        if self.vo.side == 0 then
            self.side = self.database.caster.side
        else
            self.side = 3-self.database.caster.side
        end

        self.inited = true
    end
    if self["update_pos_by_"..self.vo.target](self,sess,delta)==false then
        print("@@return true")
        return "completed"
    end
    self["update_track_by_"..self.vo.track](self,sess,delta)
    if self.process > self.vo.tick then
        local units = self["update_shape_by_"..self.vo.shape](self,sess,delta)

        if self.vo.can_repeat == false then
            for _,u in ipairs(units) do
                table.insert( self.database.target_trace,u.uid )
            end
        end
        print("@@aoe targets:"..#units.." caster:"..self.database.caster.uid.." target:"..self.targets[1].uid)
        for _,u in ipairs(units) do
            print("@@unit id:"..u.uid)
            for _, v in ipairs(self.raw_skills) do
                v:execute(sess,u) 
            end
        end

        self.process = 0
    end

    self.timepass = self.timepass + delta
    self.process = self.process + delta
    if self.timepass > self.vo.duration and self.vo.duration ~= -1 then
        return "completed"
    end
end

------------target
function this:update_pos_by_unit( sess,delta )
    if self.targets[1].alive ~= 0 then return false end
    self.target_pos.X = self.targets[1].transform.grid_pos.X
    self.target_pos.Y = self.targets[1].transform.grid_pos.Y
    return true
end

function this:update_pos_by_pos( sess,delta )
    return true
end

------------track

function this:update_track_by_fixed( sess,delta )
    return
end

function this:update_track_by_straight( sess,delta )
    -- body
end

------------shape

function this:update_shape_by_circle( sess,delta )
    local target = self.targets[1]
    local func = function(unit)
        local flag = false
        if sess.field:distance(target,unit) <= self.vo.radius then
            flag = true
        end
        return self:check_repeat()(unit) and flag
    end
    return sess.field:get_targets(self.opposite_type,self.vo.with_structure,self.side,self.database.caster,-1,func)
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