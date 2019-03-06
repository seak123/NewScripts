local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local effect = require("module.battle.skill.raw_skill_vo.effect_vo")
local property = require("module.battle.skill.raw_skill_vo.property_change_vo")
local buff = require("module.battle.skill.raw_skill_vo.buff_vo")
local calc = require("module.battle.skill.utils.caculate")
local decorator = require("module.battle.unit.behavior_tree.decorator")
local check = require("module.battle.skill.utils.checkers")
local message = require("module.battle.skill.raw_skill_vo.message_vo")
local this = {}



local prop0 = property.new()
prop0.prop_name = "attack_rateadd"
prop0.calc = calc.make_common_calc(60)

local effect0 = effect.new()
effect0.effect_id = 1091
effect0.execute_pos = effect.ExecutePos.Caster
effect0.attach = true


local sp_buff = buff.new()
sp_buff.buff_id = 1091
sp_buff.duration = 4
sp_buff.execute_type = 0
sp_buff.max_stack = 1
-- 2bit: 11
sp_buff.feature = 3
sp_buff.checkers = {check.check_chance(0.1)}
sp_buff.buff_occasion = "on_attack"
sp_buff:append("belongs",prop0,effect0)

local buff0 = buff.new()
buff0.buff_id = 1090
buff0.duration = -1
-- 2bit: 01
buff0.feature = 1
buff0.execute_type = 0
buff0:append("belongs",sp_buff)

buff0.skill_type = "passive"

return buff0