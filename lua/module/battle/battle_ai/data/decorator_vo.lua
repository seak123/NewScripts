local base = require("lua.module.battle.battle_ai.data.base_vo")
local this = class("decorator_vo",base)

this.execute ="module.battle.battle_ai.ai_decorator"

this.Type = {
    CardAvaliable = "CardAvaliable",
    CardPriority = "CardPriority"
}

--this.type = this.Type.Foward

return this