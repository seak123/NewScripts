local base = require("module.battle.skill.ripe_skill_vo.base_ripeskill_vo")
local this = class("delay_skill_vo",base)


this.execute = "module.battle.skill.ripe_skill.delay_skill"
this.delay = 0

return this