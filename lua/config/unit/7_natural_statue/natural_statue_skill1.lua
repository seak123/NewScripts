local throw_skill = require("module.battle.skill.ripe_skill_vo.throw_skill_vo")
local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local delay_skill = require("module.battle.skill.ripe_skill_vo.delay_skill_vo")
local heal = require("module.battle.skill.raw_skill_vo.heal_vo")
local effect = require("module.battle.skill.raw_skill_vo.effect_vo")
local property = require("module.battle.skill.raw_skill_vo.property_change_vo")
local buff = require("module.battle.skill.raw_skill_vo.buff_vo")
local calc = require("module.battle.skill.utils.caculate")
local decorator = require("module.battle.unit.behavior_tree.decorator")
local retarget = require("module.battle.skill.ripe_skill_vo.retarget_skill_vo")
local this = {}



local heal0 = heal.new()
heal0.calc = calc.make_common_calc(300) 


local prop0 = property.new()
prop0.prop_name = "defenceadd"
prop0.calc = calc.make_common_calc(25)

local prop1 = property.new()
prop1.prop_name = "magic_resistadd"
prop1.calc = calc.make_common_calc(0.25)

local effect0 = effect.new()
effect0.effect_id = 2
effect0.execute_pos = effect.ExecutePos.Caster
effect0.attach = true

local buff0 = buff.new()
buff0.buff_id = 70
buff0.duration = 10
buff0.execute_type = 0
buff0:append("belongs",effect0,prop0,prop1)

local normal1 = normal_skill.new()
normal1:append("raw_skills",buff0,heal)

local retarget0 = retarget.new()
retarget0.target_type = retarget.TargetType.Random
retarget0.can_repeat = false
retarget0.num = 2
retarget0.append("childs",normal1)

local delay0 = delay_skill.new()
delay0.delay = 1
delay0:append("childs",retarget0)

local effect0 = effect.new()
effect0.effect_id = 1

local normal0 = normal_skill.new()
normal0:append("raw_skills",effect0)
normal0:append("childs",delay0)


this.root = {retarget0}
this.decorators = {decorator.find_alive_friend(false)}
this.coold = 10

return this