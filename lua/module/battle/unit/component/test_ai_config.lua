local behavior = require("module.battle.unit.behavior_tree.data.behavior_vo")
local action = require("module.battle.unit.behavior_tree.data.action_vo")
local decorator = require("module.battle.unit.behavior_tree.data.decorator_vo")




local ac_attack = action.new()
ac_attack.action_type = action.ACTION.Attack

local ac_move = action.new()
ac_move.action_type = action.ACTION.MoveUnit

local de_find = decorator.new()
de_find.type = decorator.Type.EnemyAround

local be_attack = behavior.new()
be_attack.controll_type = "seq"
be_attack:append("subs",ac_attack,ac_move)
be_attack:append("decorators",de_find)
-----------------------------
local ac_move0 = action.new()
ac_move0.action_type = action.ACTION.Move

local de_forward = decorator.new()
de_forward.type = decorator.Type.Foward

local be_forward = behavior.new()
be_forward.controll_type = "seq"
be_forward:append("subs",ac_move0)
be_forward:append("decorators",de_forward)

------------------------------
local be_root = behavior.new()
be_root.controll_type = "sel"
be_root:append("subs",be_attack,be_forward)

return be_root