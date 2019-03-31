local behavior = require("module.battle.unit.behavior_tree.data.behavior_vo")
local action = require("module.battle.unit.behavior_tree.data.action_vo")
local decorator = require("module.battle.unit.behavior_tree.data.decorator_vo")

-------------------------------------------
local ac_appear = action.new()
ac_appear.action_type = action.ACTION.Appear
ac_appear.priority = 2
----------------------------------------

-----------------------------
local ac_idle0 = action.new()
ac_idle0.action_type = action.ACTION.Idle

local be_stay = behavior.new()
be_stay.controll_type = "seq"
be_stay:append("subs",ac_idle0)
------------------------------
local be_root = behavior.new()
be_root.controll_type = "sel"
be_root:append("subs",be_stay,ac_appear)

return be_root