
local base = require("module.battle.skill.raw_skill_vo.base_rawskill_vo")
local this = class("property_change_vo",base)

this.ValueType = {
    Additive = 1,
    Override =2
}

this.execute = "module.battle.skill.raw_skill.property_change"
this.value_type = this.ValueType.Override
this.prop_name = nil


return this