
local this = {}

local buff = require("module.battle.skill.raw_skill_vo.buff_vo")
local retarget = require("module.battle.skill.ripe_skill_vo.retarget_skill_vo")
local damage = require("module.battle.skill.raw_skill_vo.damage_vo")
local calc = require("module.battle.skill.utils.caculate")
local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local effect = require("module.battle.skill.raw_skill_vo.effect_vo")
local throw_skill = require("module.battle.skill.ripe_skill_vo.throw_skill_vo")

local effect1 = effect.new()
effect1.effect_id = 1081
effect1.clean_delay = 0.2

local damage1 = damage.new()
damage1.calc = calc.make_common_attack(1, 0) 

local normal1 = normal_skill.new()
normal1:append("raw_skills",damage1)

local throw1 = throw_skill.new()
throw1.speed = 320
throw1.trace = throw_skill.Trace.Curve
throw1:append("effect",effect1)
throw1:append("childs",normal1)

local retarget0 = retarget.new()
retarget0.target_type = retarget.TargetType.Distance
retarget0.can_repeat = false
retarget0.cantain_curr_target = false
retarget0.num = 2
retarget0.distance = 120
retarget0.buff_occasion = "on_attack"
retarget0:append("childs",throw1)

local buff0 = buff.new()
buff0.buff_id = 1081
buff0.duration = -1
-- 2bit: 01
buff0.feature = 1
buff0.execute_type = 0
buff0:append("belongs",retarget0)


buff0.skill_type = "passive"

return buff0