
local base = require("module.battle.skill.row_skill_vo.base_rowskill_vo")
local this = class("damage_vo",base)

this.execute = "module.battle.skill.row_skill.damage"

return this