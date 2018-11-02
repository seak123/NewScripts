local base = require("module.battle.skill.ripe_skill.base_skill")
local this = class("normal_skill",base)

function this:ctor( vo,database )
    self.database = database
    self.targets = {}
    self:build_to_array("raw_skills",vo.raw_skills)
    self:build_to_array("childs",vo.childs)
    self:build_to_array("brothers",vo.brothers)
end

function this:execute( sess,delta )
    for _, v in ipairs(self.raw_skills) do
        v:execute(sess,delta,self.database,self.targets[1]) 
    end
    return "completed"
end

return this