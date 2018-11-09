
local base = require("module.battle.skill.raw_skill_vo.base_rawskill_vo")
local this = class("summon_vo",base)

this.execute = "module.battle.skill.raw_skill.summon"

this.unit_id = 0
this.unit_num = 0

return this