local throw_skill = require("module.battle.skill.ripe_skill_vo.throw_skill_vo")
local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local damage = require("module.battle.skill.raw_skill_vo.damage_vo")
local effect = require("module.battle.skill.raw_skill_vo.effect_vo")
local property = require("module.battle.skill.raw_skill_vo.property_change_vo")
local buff = require("module.battle.skill.raw_skill_vo.buff_vo")
local retarget = require("module.battle.skill.ripe_skill_vo.retarget_skill_vo")
local calc = require("module.battle.skill.utils.caculate")
local decorator = require("module.battle.unit.behavior_tree.decorator")
local delay = require("module.battle.skill.ripe_skill_vo.delay_skill_vo")
local this = {}




local damage0 = damage.new()
damage0.calc = calc.make_common_attack(1, 0)
damage0.damage_source = damage.DamageSource.Skill

local effect0 = effect.new()
effect0.effect_id = 6022
effect0.execute_pos = effect.ExecutePos.Target
effect0.attach = false

local effect1 = effect.new()
effect1.effect_id = 6021
effect1.execute_pos = effect.ExecutePos.Caster
effect1.attach = true

local normal0 = normal_skill.new()
normal0:append("raw_skills",damage0,effect0)

local normal1 = normal_skill.new()
normal1:append("raw_skills",effect1)

local delay4 = delay.new()
delay4.delay = 0.5
delay4:append("childs",normal0)

local retarget1 = retarget.new()
retarget1.target_type = retarget.TargetType.Distance
retarget1.cantain_curr_target = false
retarget1.can_repeat = false
retarget1.num = 1
retarget1.distance = 80
retarget1.on_target = 1
retarget1.target_source = 0
retarget1:append("childs",delay4)

local delay3 = delay.new()
delay3.delay = 0.5
delay3:append("childs",normal0,retarget1)

local retarget0 = retarget.new()
retarget0.target_type = retarget.TargetType.Distance
retarget0.cantain_curr_target = false
retarget0.can_repeat = false
retarget0.num = 1
retarget0.distance = 80
retarget0.on_target = 1
retarget0.target_source = 0
retarget0:append("childs",delay3)

local delay0 = delay.new()
delay0.delay = 0.2
delay0:append("childs",normal0,retarget0)


local delay1 = delay.new()
delay1.delay = 0.1
delay1:append("childs",normal1)

this.root = {delay0,delay1}
this.decorators = {decorator.check_skill_EnemyInRange(true,1)}
this.range = 1
this.coold = 4
this.energy = 0

return this