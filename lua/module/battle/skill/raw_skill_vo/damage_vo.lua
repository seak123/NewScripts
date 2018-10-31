
local base = require("module.battle.skill.raw_skill_vo.base_rawskill_vo")
local this = class("damage_vo",base)

this.execute = "module.battle.skill.raw_skill.damage"

--0,physical 1,magic 2,real
this.damage_type = 0

return this