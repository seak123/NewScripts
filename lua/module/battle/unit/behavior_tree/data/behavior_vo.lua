
local base = require("module.battle.unit.behavior_tree.base_node_vo")
local this = class("behavior_vo",base)

this.execute = "module.battle.unit.behavior_tree.behavior_node"


-- sel;par;seq;
this.controll_type = "sel"


return this