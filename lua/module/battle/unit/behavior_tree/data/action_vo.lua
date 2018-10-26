local base = require("module.battle.unit.behavior_tree.base_node_vo")
local this = class("action_vo",base)

this.ACTION = {
    Move = "Move",
    Attack = "Attack",
    MoveUnit = "MoveUnit"
}
this.execute = "module.battle.unit.behavior_tree.action_node"


this.action_type = this.ACTION.Move


return this