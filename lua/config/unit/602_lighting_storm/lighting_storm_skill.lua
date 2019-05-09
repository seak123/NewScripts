local throw_skill = require("module.battle.skill.ripe_skill_vo.throw_skill_vo")
local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local damage = require("module.battle.skill.raw_skill_vo.damage_vo")
local effect = require("module.battle.skill.raw_skill_vo.effect_vo")
local property = require("module.battle.skill.raw_skill_vo.property_change_vo")
local buff = require("module.battle.skill.raw_skill_vo.buff_vo")
local calc = require("module.battle.skill.utils.caculate")
local decorator = require("module.battle.unit.behavior_tree.decorator")
local delay = require("module.battle.skill.ripe_skill_vo.delay_skill_vo")
local this = {}



local damage0 = damage.new()
damage0.calc = calc.make_common_attack(1, 0) 

local effect0 = effect.new()
effect0.effect_id = 6022
effect0.execute_pos = effect.ExecutePos.Target
effect0.attach = false

local normal0 = normal_skill.new()
normal0:append("raw_skills",damage0,effect0)

local delay0 = delay.new()
delay0.delay = 0.2
delay0:append("childs",normal0)

this.root = {delay0}
this.decorators = {decorator.check_skill_EnemyInRange(true,1)}
this.coold = 4
this.energy = 0

return this