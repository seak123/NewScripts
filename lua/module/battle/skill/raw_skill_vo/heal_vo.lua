
local base = require("module.battle.skill.raw_skill_vo.base_rawskill_vo")
local this = class("heal_vo",base)


this.execute = "module.battle.skill.raw_skill.heal"

this.calc = nil

return this