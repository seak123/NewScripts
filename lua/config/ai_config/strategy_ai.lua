local behavior = require("module.battle.unit.behavior_tree.data.behavior_vo")
local action = require("module.battle.unit.behavior_tree.data.action_vo")
local decorator = require("module.battle.unit.behavior_tree.data.decorator_vo")

-------------------------------------------
local ac_appear = action.new()
ac_appear.action_type = action.ACTION.Appear
ac_appear.priority = 3
----------------------------------------

-----------------------------
local ac_idle0 = action.new()
ac_idle0.action_type = action.ACTION.Idle

local be_stay = behavior.new()
be_stay.controll_type = "seq"
be_stay:append("subs",ac_idle0)
----------------------------------

local ac_wander = action.new()
ac_wander.action_type = action.ACTION.MoveToPos

local de_wander = decorator.new()
de_wander.type = decorator.Type.Boring

local be_wander = behavior.new()
be_wander.controll_type = "sel"
be_wander.priority = 2
be_wander:append("subs",ac_wander)
be_wander:append("decorators",de_wander)


------------------------------
local be_root = behavior.new()
be_root.controll_type = "sel"
be_root:append("subs",be_wander,be_stay,ac_appear)

return be_root