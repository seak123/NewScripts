local base = require("lua.module.battle.skill.ripe_skill.base_skill")
local this = class("normal_skill")

function this:ctor( vo,database )
    self.database = database
    self:build_to_array("raw_skills",vo.raw_skills)
    self:build_to_array("childs",vo.childs)
end

function this:execute( sess,delta )
    for _, v in ipairs("raw_skill") do
        v:execute(sess,delta,self.database)
    end
    return true
end

return this