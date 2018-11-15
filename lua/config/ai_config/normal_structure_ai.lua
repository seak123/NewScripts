local behavior = require("module.battle.unit.behavior_tree.data.behavior_vo")
local action = require("module.battle.unit.behavior_tree.data.action_vo")
local decorator = require("module.battle.unit.behavior_tree.data.decorator_vo")

local ac_caster = action.new()
ac_caster.action_type = ac_caster.ACTION.Caster

local de_skill = decorator.new()
de_skill.type = decorator.Type.SkillAvaliable

local be_caster = behavior.new()
be_caster.controll_type = "sel"
be_caster.priority = 1
be_caster:append("subs",ac_caster)
be_caster:append("decorators",de_skill)

local ac_appear = action.new()
ac_appear.action_type = action.ACTION.Appear
ac_appear.priority = 2

------------------------------
local be_root = behavior.new()
be_root.controll_type = "sel"
be_root:append("subs",ac_appear,be_caster)

return be_root