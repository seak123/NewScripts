local behavior = require("module.battle.unit.behavior_tree.data.behavior_vo")
local action = require("module.battle.unit.behavior_tree.data.action_vo")
local decorator = require("module.battle.unit.behavior_tree.data.decorator_vo")

local ac_caster = action.new()
ac_caster.action_type = ac_caster.ACTION.Caster

local de_skill = decorator.new()
de_skill.type = decorator.Type.SkillAvaliable

local be_caster = behavior.new()
be_caster.controll_type = "sel"
be_caster.priority = 4
be_caster:append("subs",ac_caster)
be_caster:append("decorators",de_skill)

------------------------------
local de_inrange = decorator.new()
de_inrange.type = decorator.Type.EnemyInAttackRange

local ac_attack = action.new()
ac_attack.priority =4
ac_attack.action_type = action.ACTION.Attack
ac_attack:append("decorators",de_inrange)

local ac_move = action.new()
ac_move.action_type = action.ACTION.MoveToEnemy

local de_find = decorator.new()
de_find.type = decorator.Type.EnemyAround
-------------------------------------------
local ac_appear = action.new()
ac_appear.action_type = action.ACTION.Appear
ac_appear.priority = 5
----------------------------------------

local be_attack = behavior.new()
be_attack.controll_type = "sel"
be_attack.priority = 3
be_attack:append("subs",ac_move,ac_attack)
be_attack:append("decorators",de_find)
-----------------------------
local ac_idle0 = action.new()
ac_idle0.action_type = action.ACTION.Idle

local be_stay = behavior.new()
be_stay.controll_type = "seq"
be_stay:append("subs",ac_idle0)
----------------------------------

local ac_back = action.new()
ac_back.action_type = action.ACTION.MoveToPos

local de_back = decorator.new()
de_back.type = decorator.Type.NeedBack

local be_back = behavior.new()
be_back.controll_type = "sel"
be_back.priority = 2
be_back:append("subs",ac_back)
be_back:append("decorators",de_back)


------------------------------
local be_root = behavior.new()
be_root.controll_type = "sel"
be_root:append("subs",be_attack,be_back,be_stay,be_caster,ac_appear)

return be_root