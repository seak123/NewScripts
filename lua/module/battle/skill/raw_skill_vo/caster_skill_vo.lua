
local base = require("module.battle.skill.raw_skill_vo.base_rawskill_vo")
local this = class("caster_skill_vo",base)

this.execute = "module.battle.skill.raw_skill.caster_skill"

-- 0,caster 1,target
this.on_target = 1 

this.skills = {}

return this