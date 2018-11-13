local throw_skill = require("module.battle.skill.ripe_skill_vo.throw_skill_vo")
local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local damage = require("module.battle.skill.raw_skill_vo.damage_vo")
local effect = require("module.battle.skill.raw_skill_vo.effect_vo")
local property = require("module.battle.skill.raw_skill_vo.property_change_vo")
local buff = require("module.battle.skill.raw_skill_vo.buff_vo")
local calc = require("module.battle.skill.utils.caculate")
local decorator = require("module.battle.unit.behavior_tree.decorator")
local this = {}



local prop0 = property.new()
prop0.prop_name = "attackadd"
prop0.calc = calc.make_common_calc(100)

local effect0 = effect.new()
effect0.effect_id = 2
effect0.execute_pos = effect.ExecutePos.Caster
effect0.attach = true

local buff0 = buff.new()
buff0.buff_id = 40
buff0.duration = 10
buff0.execute_type = 0
buff0:append("belongs",effect0,prop0)

local normal = normal_skill.new()
normal:append("raw_skills",buff0)

this.root = {normal}
this.decorators = {decorator.check_skill_EnemyInRange(150,true)}
this.coold = 20

return this