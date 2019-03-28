local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local effect = require("module.battle.skill.raw_skill_vo.effect_vo")
local property = require("module.battle.skill.raw_skill_vo.property_change_vo")
local buff = require("module.battle.skill.raw_skill_vo.buff_vo")
local calc = require("module.battle.skill.utils.caculate")
local decorator = require("module.battle.unit.behavior_tree.decorator")
local check = require("module.battle.skill.utils.checkers")
local message = require("module.battle.skill.raw_skill_vo.message_vo")
local aoe = require("module.battle.skill.ripe_skill_vo.aoe_skill_vo")
local damage = require("module.battle.skill.raw_skill_vo.damage_vo")
local state = require("module.battle.skill.raw_skill_vo.state_vo")
local caster = require("module.battle.skill.raw_skill_vo.caster_skill_vo")
local this = {}


local aoe_dam = damage.new()
aoe_dam.damage_source = damage.DamageSource.Skill
aoe_dam.calc = calc.make_common_attack(0.5,0)

local state0 = state.new()
state0.state = state.State.Cold

local buff3 = buff.new()
buff3.buff_id = 5012
buff3.duration = 4
-- 2bit: 10
buff3.feature = 2
buff3.execute_type = 1
buff3:append("belongs",state0)

local aoe0 = aoe.new()
aoe0.can_repeat = false
aoe0.tick = -1
aoe0.duration = 0
aoe0.radius = 20
aoe0:append("raw_skills",aoe_dam,buff3)

local normal = normal_skill.new()
normal:append("raw_skills",buff3)

local caster0 = caster.new()
caster0.on_target = 1
caster0.buff_occasion = "pre_normal_damage"
caster0.skills = {normal,aoe0}

local buff0 = buff.new()
buff0.buff_id = 5011
buff0.duration = -1
-- 2bit: 01
buff0.feature = 1
buff0.execute_type = 0
buff0:append("belongs",caster0)


buff0.skill_type = "passive"

return buff0