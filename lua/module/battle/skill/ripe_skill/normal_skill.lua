local base = require("module.battle.skill.ripe_skill.base_skill")
local this = class("normal_skill",base)

function this:ctor( vo,database )
    self.database = database
    self.targets = {}
    self:build_to_array("raw_skills",vo.raw_skills,database)
    self:build_to_array("childs",vo.childs,database)
end

function this:execute( sess,delta )
    if self.targets[1]~= nil and self.targets[1].alive ~= 0 then return "completed" end
    for _, v in ipairs(self.raw_skills) do
        v:execute(sess,self.targets[1]) 
    end
    return "completed"
end

return this