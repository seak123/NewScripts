
local base = require("lua.module.battle.unit.behavior_tree.base_node")
local this = class("behavior_vo",base)

function this:ctor(  )
     -- sel;par;seq;
     self.controll_type = "sel"
end

return this