
local this = {}

local buff = require("module.battle.skill.raw_skill_vo.buff_vo")
local retarget = require("module.battle.skill.ripe_skill_vo.retarget_skill_vo")
local damage = require("module.battle.skill.raw_skill_vo.damage_vo")
local calc = require("module.battle.skill.utils.caculate")
local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local effect = require("module.battle.skill.raw_skill_vo.effect_vo")
local throw_skill = require("module.battle.skill.ripe_skill_vo.throw_skill_vo")
local caster = require("module.battle.skill.raw_skill_vo.caster_skill_vo")
local common_buff = require("module.battle.skill.raw_skill_vo.common_buff_vo")
local check = require("module.battle.skill.utils.checkers")

local effect1 = effect.new()
effect1.effect_id = 1082
effect1.execute_pos = effect.ExecutePos.Caster
effect1.attach = true

local sp_buff = common_buff.new()
common_buff.buff_type = common_buff.BuffType.Strength
common_buff.stack_num = calc.make_common_level(2,20)

local normal1 = normal_skill.new()
normal1:append("raw_skills",sp_buff,effect1)

local caster0 = caster.new()
caster0.buff_occasion = "on_attack"
caster0.checkers = {check.check_chance(0.2)}
caster0.on_target = 0
caster0.skills = {normal1}

local buff0 = buff.new()
buff0.buff_id = 1081
buff0.duration = -1
-- 2bit: 01
buff0.feature = 1
buff0.execute_type = 0
buff0:append("belongs",caster0)


buff0.skill_type = "passive"

return buff0