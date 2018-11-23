local base = require("module.battle.skill.ripe_skill_vo.base_ripeskill_vo")
local this = class("aoe_skill_vo",base)

this.execute = "module.battle.skill.ripe_skill.aoe_skill"

this.Shape = {
    Circle = 1
}

this.Track = {
    Fixed = 1
}

this.Target = {
    Unit = 1,
    Pos = 2
}

this.can_repeat = true
this.tick = 1
this.duration = 1

--circle
this.radius = 1

return this