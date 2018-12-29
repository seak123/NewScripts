
local base = require("module.battle.skill.raw_skill_vo.base_rawskill_vo")
local this = class("state_vo",base)

this.State = {
    Cold = 1,
    Freezen = 2
}

this.execute = "module.battle.skill.raw_skill_vo.view_state_vo"

-- this.effect_id = 0
-- this.execute_pos = this.ExecutePos.Caster
-- this.attach = false
-- this.clean_delay = 0



return this