local base = require("module.battle.skill.ripe_skill_vo.base_ripeskill_vo")
local this = class("throw_skill_vo",base)
this.Trace = {
    Straight = 1,
    Curve = 2
}

this.execute = "module.battle.skill.ripe_skill.throw_skill"

this.trace = this.Trace.Straight
this.effect = nil

return this