local base = require("module.battle.skill.ripe_skill_vo.base_ripeskill_vo")
local this = class("aoe_skill_vo",base)

this.execute = "module.battle.skill.ripe_skill.aoe_skill"

this.Shape = {
    Circle = "circle"
}

this.Track = {
    Fixed = "fixed",
    Straight = "straight",

}

this.Target = {
    Unit = "unit",
    Pos = "pos"
}

this.shape = this.Shape.Circle
this.track = this.Track.Fixed
this.target = this.Target.Unit

this.can_repeat = true
this.cantain_curr_target = false
-- tick = -1 means update every frame
this.tick = 1
-- duration = -1 means forever ; duration = 0 then means once
this.duration = 1

this.with_structure = false

-- side: 0, friend 1,enemy
this.side = 1

--circle
this.radius = 1

return this