local base = require("module.battle.skill.ripe_skill_vo.base_ripeskill_vo")
local this = class("throw_skill_vo",base)
this.Trace = {
    Straight = "straight",
    Curve = "curve"
}
this.Target = {
    Unit = 1,
    Pos = 2
}

this.execute = "module.battle.skill.ripe_skill.throw_skill"

this.trace = this.Trace.Straight
this.speed = 10
-- accelerated speed
this.a = 10
this.effect = nil
this.target_type = this.Target.Unit

this.target_socket = "S_Bottom"

return this