
local base = require("module.battle.skill.raw_skill_vo.base_rawskill_vo")
local this = class("effect_vo",base)

this.ExecutePos = {
    Caster = 1,
    Target = 2,
    CasterPos =3,
    TargetPos =4
}

this.execute = "module.battle.skill.raw_skill.effect"

this.effect_id = 0
this.execute_pos = this.ExecutePos.Caster
this.attach = false



return this