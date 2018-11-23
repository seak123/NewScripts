local base = require("module.battle.skill.ripe_skill.base_skill")
local this = class("aoe_skill",base)
local aoe_vo = require("lua.module.battle.skill.ripe_skill_vo.aoe_skill_vo")

function this:ctor( vo,database )
    self.vo = vo
    self.database = database
    self.targets = {}
    self:build_to_array("raw_skills",vo.raw_skills,database)
    --self:build_to_array("childs",vo.childs,database)
    self.process = 0
end


function this:execute( sess,delta )
    if self.vo.target_type == aoe_vo.Target.Unit then
        if self.targets[1].alive ~= 0 then return "completed" end 
        self.target_pos.X = self.targets[1].transform.grid_pos.X
        self.target_pos.Y = self.targets[1].transform.grid_pos.Y
    elseif self.vo.target_type == aoe_vo.Target.Pos then
        self.target_pos.X = self.database.target_pos.X
        self.target_pos.Y = self.database.target_pos.Y
    end
    -- self["update_track_by"..]
    -- return "completed"
end

return this