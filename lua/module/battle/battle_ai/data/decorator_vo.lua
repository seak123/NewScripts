local base = require("module.battle.battle_ai.data.base_vo")
local this = class("decorator_vo",base)

this.execute ="module.battle.battle_ai.ai_decorator"

this.Type = {
    CalcPriority = "CalcPriority"
    --CardAvaliable = "CardAvaliable"
}

this.type = this.Type.CalcPriority

return this