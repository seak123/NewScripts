local throw_skill = require("module.battle.skill.ripe_skill_vo.throw_skill_vo")
local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local retarget = require("module.battle.skill.ripe_skill_vo.retarget_skill_vo")
local damage = require("module.battle.skill.raw_skill_vo.damage_vo")
local effect = require("module.battle.skill.raw_skill_vo.effect_vo")
local calc = require("module.battle.skill.utils.caculate")
local this = {}


local damage0 = damage.new()
damage0.calc = calc.make_common_attack(1, 0) 

local normal = normal_skill.new()
normal:append("raw_skills",damage0)

local effect0 = effect.new()
effect0.effect_id = 10001
effect0.clean_delay = 0.2

local throw = throw_skill.new()
throw.speed = 320
throw.trace = throw_skill.Trace.Curve
throw:append("effect",effect0)
throw:append("childs",normal)



this.root = {throw}

return this