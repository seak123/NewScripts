
local base = require("module.battle.battle_ai.data.base_vo")
local this = class("behavior_vo",base)

this.execute = "module.battle.battle_ai.ai_behavior"


-- sel;par;seq;
this.controll_type = "sel"
this.priority = 1

return this