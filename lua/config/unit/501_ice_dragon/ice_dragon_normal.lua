local throw_skill = require("module.battle.skill.ripe_skill_vo.throw_skill_vo")
local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local retarget = require("module.battle.skill.ripe_skill_vo.retarget_skill_vo")
local damage = require("module.battle.skill.raw_skill_vo.damage_vo")
local effect = require("module.battle.skill.raw_skill_vo.effect_vo")
local calc = require("module.battle.skill.utils.caculate")
local aoe = require("module.battle.skill.ripe_skill_vo.aoe_skill_vo")
local buff = require("module.battle.skill.raw_skill_vo.buff_vo")
local prop = require("module.battle.skill.raw_skill_vo.property_change_vo")
local state = require("module.battle.skill.raw_skill_vo.state_vo")
local this = {}

local aoe_dam = damage.new()
aoe_dam.calc = calc.make_common_attack(0.5,0)

-- local prop0 = prop.new()
-- prop0.prop_name = "attack_rateadd"
-- prop0.calc = calc.make_common_calc(-1)

-- local prop1 = prop.new()
-- prop1.prop_name = "speedrate"
-- prop1.calc = calc.make_common_calc(-0.5)

local state0 = state.new()
state0.state = state.State.Cold

local buff0 = buff.new()
buff0.buff_id = 5011
buff0.duration = 4
-- 2bit: 10
buff0.feature = 2
buff0.execute_type = 1
buff0:append("belongs",state0)

local aoe0 = aoe.new()
aoe0.can_repeat = false
aoe0.tick = -1
aoe0.duraton = 0
aoe0.opposite_type = 4
aoe0.radius = 20
aoe0:append("raw_skills",aoe_dam,buff0)

local damage0 = damage.new()
damage0.calc = calc.make_common_attack(1, 0) 

-- aoe effect
local effect1 = effect.new()
effect1.effect_id = 5012

local normal = normal_skill.new()
normal:append("raw_skills",damage0,buff0)

local effect0 = effect.new()
effect0.effect_id = 5011
effect0.clean_delay = 0.5

local throw = throw_skill.new()
throw.speed = 120
throw.trace = throw_skill.Trace.Curve
throw:append("effect",effect0)
throw:append("childs",normal,aoe0)



this.root = {throw}

return this