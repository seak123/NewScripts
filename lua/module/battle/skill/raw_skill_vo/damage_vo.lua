
local base = require("module.battle.skill.raw_skill_vo.base_rawskill_vo")
local this = class("damage_vo",base)

this.DamageType = {
    Physical = 1,
    Magic = 2,
    Real = 3
}

this.DamageSource = {
    Attack = 1,
    Skill = 2,
}

this.execute = "module.battle.skill.raw_skill.damage"

this.damage_source = this.DamageSource.Attack
--0,physical 1,magic 2,real
this.damage_type = this.DamageType.Physical
this.calc = nil

return this