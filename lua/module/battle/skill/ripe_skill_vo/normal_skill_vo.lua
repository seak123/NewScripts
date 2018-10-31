local base = require("module.battle.skill.ripe_skill_vo.base_ripeskill_vo")
local this = class("normal_skill_vo",base)

this.raw_skills = {}
this.execute = "module.battle.skill.ripe_skill.normal_skill"

return this