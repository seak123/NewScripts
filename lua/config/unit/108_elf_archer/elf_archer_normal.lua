local throw_skill = require("module.battle.skill.ripe_skill_vo.throw_skill_vo")
local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local retarget = require("module.battle.skill.ripe_skill_vo.retarget_skill_vo")
local damage = require("module.battle.skill.raw_skill_vo.damage_vo")
local effect = require("module.battle.skill.raw_skill_vo.effect_vo")
local calc = require("module.battle.skill.utils.caculate")
local this = {}

local effect1 = effect.new()
effect1.effect_id = 1

local damage1 = damage.new()
damage1.calc = calc.make_common_attack(1, 0) 

local normal1 = normal_skill.new()
normal1:append("raw_skills",damage1)

local throw1 = throw_skill.new()
throw1.speed = 60
throw1.trace = throw_skill.Trace.Curve
throw1:append("effect",effect1)
throw1:append("childs",normal1)


-- local retarget0 = retarget.new()
-- retarget0.target_type = retarget.TargetType.Distance
-- retarget0.can_repeat = false
-- retarget0.cantain_curr_target = false
-- retarget0.num = 2
-- retarget0.distance = 100
-- retarget0.append("childs",normal1)

local retarget0 = retarget.new()
retarget0.target_type = retarget.TargetType.Random
retarget0.can_repeat = false
retarget0.cantain_curr_target = false
retarget0.num = 2
retarget0.distance = 200
retarget0:append("childs",throw1)

local damage0 = damage.new()
damage0.calc = calc.make_common_attack(1, 0) 

local normal = normal_skill.new()
normal:append("raw_skills",damage0)

local effect0 = effect.new()
effect0.effect_id = 1

local throw = throw_skill.new()
throw.speed = 60
throw.trace = throw_skill.Trace.Curve
throw:append("effect",effect0)
throw:append("childs",normal)



this.root = {throw,retarget0}

return this