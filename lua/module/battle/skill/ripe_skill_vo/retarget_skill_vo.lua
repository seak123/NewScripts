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
-- 0 master is caster 1 master is target
this.on_target = 0
-- 0 target select from database 1 target select from battle trace
this.target_source = 0

this.distance = 80

return this