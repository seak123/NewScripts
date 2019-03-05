
local base = require("module.battle.skill.raw_skill_vo.base_rawskill_vo")
local this = class("caster_skill_vo",base)

this.execute = "module.battle.skill.raw_skill.caster_skill"

this.skills = {}

return this