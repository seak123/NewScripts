
local base = require("module.battle.skill.raw_skill_vo.base_rawskill_vo")
local this = class("modify_buff_vo",base)

this.Type = {
    Dispel = "dispel",
    Destroy = "destroy"
}

this.execute = "module.battle.skill.raw_skill.modify_buff"

this.mdf_type = this.Type.Dispel

this.count = 1

return this