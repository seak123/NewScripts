local base = require("module.battle.unit.behavior_tree.base_node_vo")
local this = class("action_vo",base)

this.ACTION = {
    MoveForward = "MoveForward",
    MoveToPos = "MoveToPos",
    Attack = "Attack",
    MoveToEnemy = "MoveToEnemy",
    Caster = "Caster",
    Appear = "Appear",
    StayBack = "StayBack",
    Idle = "Idle"
}
this.execute = "module.battle.unit.behavior_tree.action_node"


this.action_type = this.ACTION.Move
this.priority = 1


return this