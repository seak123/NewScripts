local base = require("module.battle.skill.ripe_skill_vo.base_ripeskill_vo")
local this = class("retarget_skill_vo",base)

this.TargetType = {
    Random = "random",
    Distance = "distance",
    RandomHurted = "random_hurted",
    Self = "self"
}

this.execute = "module.battle.skill.ripe_skill.retarget_skill"

this.target_type = this.TargetType.Random
this.can_repeat = true
this.cantain_curr_target = false
this.is_friend = false
this.num = 1

this.distance = 80

return this