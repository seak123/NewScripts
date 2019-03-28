local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local effect = require("module.battle.skill.raw_skill_vo.effect_vo")
local property = require("module.battle.skill.raw_skill_vo.property_change_vo")
local buff = require("module.battle.skill.raw_skill_vo.buff_vo")
local calc = require("module.battle.skill.utils.caculate")
local decorator = require("module.battle.unit.behavior_tree.decorator")
local check = require("module.battle.skill.utils.checkers")
local aoe = require("module.battle.skill.ripe_skill_vo.aoe_skill_vo")
local message = require("module.battle.skill.raw_skill_vo.message_vo")
local this = {}



local prop0 = property.new()
prop0.prop_name = "attackrate"
prop0.calc = calc.make_common_calc(0.25)

local effect0 = effect.new()
effect0.effect_id = 1091
effect0.execute_pos = effect.ExecutePos.Target
effect0.attach = true

local sp_buff = buff.new()
sp_buff.buff_id = 10012
sp_buff.duration = 1.1
sp_buff.execute_type = 0
sp_buff.max_stack = 1
-- 2bit: 01
sp_buff.feature = 1
sp_buff.checkers = {check.check_attack_range(false)}
sp_buff:append("belongs",prop0,effect0)

local aoe0 = aoe.new()
aoe0.can_repeat = true
aoe0.tick = -1
aoe0.duraton = 0
aoe0.radius = 120
aoe0.buff_occasion = "on_tick"
aoe0.interval = 1
aoe0:append("raw_skills",sp_buff)

local buff0 = buff.new()
buff0.buff_id = 10011
buff0.duration = -1
-- 2bit: 01
buff0.feature = 1
buff0.execute_type = 0
buff0:append("belongs",aoe0)

return buff0