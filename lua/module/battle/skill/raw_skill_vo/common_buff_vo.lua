
local base = require("module.battle.skill.raw_skill_vo.base_rawskill_vo")
local this = class("common_buff_vo",base)

this.execute = "module.battle.skill.raw_skill.common_buff"

this.BuffType = {
    Poison = "Poison",
    Strength = "Strength"
}
this.execute_type = 0
this.buff_type = this.BuffType.Poison
this.stack_num = 1

return this