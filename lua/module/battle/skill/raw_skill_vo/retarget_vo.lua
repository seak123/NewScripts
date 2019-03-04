
local base = require("module.battle.skill.raw_skill_vo.base_rawskill_vo")
local this = class("retarget_vo",base)

this.TargetType = {
    Random = "random",
    Distance = "distance",
    RandomHurted = "random_hurted",
    Self = "self"
}

this.execute = "module.battle.skill.raw_skill.retarget"

this.target_type = this.TargetType.Random

this.cantain_curr_target = false
this.num = 1

this.distance = 80

return this