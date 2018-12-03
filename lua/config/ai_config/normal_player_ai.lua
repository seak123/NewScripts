local behavior = require("module.battle.battle_ai.data.behavior_vo")
local action = require("module.battle.battle_ai.data.action_vo")
local decorator = require("module.battle.battle_ai.data.decorator_vo")

local ac_caster = action.new()
ac_caster.action_type = action.ACTION.Caster

local de_calc = decorator.new()
de_calc = decorator.Type.CalcPriority

local root = behavior.new()
root.controll_type = "sel"
root:append("subs",ac_caster)
root:append("decorators",de_calc)

return root