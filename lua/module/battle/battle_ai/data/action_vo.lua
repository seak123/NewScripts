local base = require("module.battle.battle_ai.data.base_vo")
local this = class("action_vo",base)

this.ACTION = {
   Caster = "Caster",
   Wait = "Wait"
}
this.execute = "module.battle.unit.behavior_tree.action_node"


--this.action_type = this.ACTION.Move
this.priority = 1


return this