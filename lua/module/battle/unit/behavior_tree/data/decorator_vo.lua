local base = require("module.battle.unit.behavior_tree.base_node_vo")
local this = class("decorator_vo",base)

this.execute = "module.battle.unit.behavior_tree.decorator"

this.Type = {
    Forward = "Forward",
    EnemyAround = "EnemyAround",
    EnemyInAttackRange = "EnemyInAttackRange"
}

this.type = this.Type.Foward

return this