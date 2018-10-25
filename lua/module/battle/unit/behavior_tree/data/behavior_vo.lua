
local base = require("module.battle.unit.behavior_tree.base_node_vo")
local this = class("behavior_vo",base)

this.execute = "module.battle.unit.behavior_tree.behavior_node"

function this:ctor(  )
     -- sel;par;seq;
     self.controll_type = "sel"
end

return this