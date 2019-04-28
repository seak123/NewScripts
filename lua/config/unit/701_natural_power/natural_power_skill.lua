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
local common_buff = require("module.battle.skill.raw_skill_vo.common_buff_vo")

local trigger = require("module.battle.trigger.vo.trigger_vo")
local this = {}

local sp_buff = common_buff.new()
common_buff.buff_type = common_buff.BuffType.Strength
common_buff.stack_num = calc.make_common_level(0,20)


local buff0 = buff.new()
buff0.buff_id = 1090
buff0.duration = -1
-- 2bit: 01
buff0.feature = 1
buff0.execute_type = 1
buff0:append("belongs",sp_buff)

local trigger0 = trigger.new()
trigger0.execute = "module.battle.trigger.simple_trigger"
trigger0.source = -1
trigger0.root = buff0
trigger0.target_type = "friend"
trigger0.occasion = "on_enter_room"


trigger0.skill_type = "trigger"

return trigger0