local throw_skill = require("module.battle.skill.ripe_skill_vo.throw_skill_vo")
local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local retarget = require("module.battle.skill.ripe_skill_vo.retarget_skill_vo")
local damage = require("module.battle.skill.raw_skill_vo.damage_vo")
local effect = require("module.battle.skill.raw_skill_vo.effect_vo")
local calc = require("module.battle.skill.utils.caculate")
local aoe = require("module.battle.skill.ripe_skill_vo.aoe_skill_vo")
local this = {}

local aoe_dam = damage.new()
aoe_dam.calc = calc.make_common_attack(0.5,0)

local aoe0 = aoe.new()
aoe0.can_repeat = false
aoe0.tick = -1
aoe0.duraton = 0
aoe0.opposite_type = 4
aoe0.radius = 20
aoe0:append("raw_skills",aoe_dam)

local damage0 = damage.new()
damage0.calc = calc.make_common_attack(1, 0) 

local normal = normal_skill.new()
normal:append("raw_skills",damage0)

local effect0 = effect.new()
effect0.effect_id = 1081
effect0.clean_delay = 0.2

local throw = throw_skill.new()
throw.speed = 320
throw.trace = throw_skill.Trace.Curve
throw:append("effect",effect0)
throw:append("childs",normal,aoe0)



this.root = {throw}

return this