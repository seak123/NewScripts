local base = require("module.battle.unit.behavior_tree.base_node_vo")
local this = class("action_vo",base)

this.ACTION = {
    MoveToPos = "MoveToPos",
    Attack = "Attack",
    MoveToUnit = "MoveToUnit"
}
this.execute = "module.battle.unit.behavior_tree.action_node"


this.action_type = this.ACTION.Move
this.priority = 1


return this