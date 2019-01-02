
local base = require("module.battle.skill.raw_skill_vo.base_rawskill_vo")
local this = class("state_vo",base)

this.State = {
    Cold = "cold",
    Frozen = "frozen"
}

this.execute = "module.battle.skill.raw_skill.state"

this.state = this.State.Cold

-- this.effect_id = 0
-- this.execute_pos = this.ExecutePos.Caster
-- this.attach = false
-- this.clean_delay = 0



return this