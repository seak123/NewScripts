local base = require("module.battle.skill.ripe_skill_vo.base_ripeskill_vo")
local this = class("retarget_skill_vo",base)

this.TargetType = {
    Random = "random",
    Distance = "distance"
}

this.execute = "module.battle.skill.ripe_skill.retarget_skill"

this.target_type = this.TargetType.Random
this.can_repeat = true
this.cantain_curr_target = false
this.num = 1

return this