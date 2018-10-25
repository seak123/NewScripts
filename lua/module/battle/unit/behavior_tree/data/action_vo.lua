local base = require("module.battle.unit.behavior_tree.base_node_vo")
local this = class("action_vo",base)

this.ACTION = {
    Move = 1,
    Attack = 2,
    MoveUnit = 3
}
this.execute = "module.battle.unit.behavior_tree.action_node"

function this:ctor(  )
    self.action_type = this.ACTION.Move
end

return this