
local base = require("module.battle.skill.raw_skill_vo.base_rawskill_vo")
local this = class("message_vo",base)


this.execute = "module.battle.skill.raw_skill.message"

this.text = ""

return this