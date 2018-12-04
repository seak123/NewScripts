local base = require("module.battle.battle_ai.data.base_vo")
local this = class("action_vo",base)

this.ACTION = {
   Caster = "Caster",
   Wait = "Wait"
}
this.execute = "module.battle.battle_ai.ai_action"


this.action_type = this.ACTION.Caster
this.priority = 1


return this