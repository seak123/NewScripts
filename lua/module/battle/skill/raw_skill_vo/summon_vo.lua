
local base = require("module.battle.skill.raw_skill_vo.base_rawskill_vo")
local this = class("summon_vo",base)

this.execute = "module.battle.skill.raw_skill.summon"
-- data can be unitdata or unit_id
this.data = 0
this.unit_num = 0

return this