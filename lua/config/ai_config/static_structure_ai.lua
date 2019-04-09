local behavior = require("module.battle.unit.behavior_tree.data.behavior_vo")
local action = require("module.battle.unit.behavior_tree.data.action_vo")
local decorator = require("module.battle.unit.behavior_tree.data.decorator_vo")

local ac_appear = action.new()
ac_appear.action_type = action.ACTION.Appear
ac_appear.priority = 3

local ac_idle = action.new()
ac_idle.action_type = action.ACTION.Idle
ac_idle.priority = 1

------------------------------
local be_root = behavior.new()
be_root.controll_type = "sel"
be_root:append("subs",ac_appear,ac_idle)

return be_root