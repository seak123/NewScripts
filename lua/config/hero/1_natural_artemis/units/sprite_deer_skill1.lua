local throw_skill = require("module.battle.skill.ripe_skill_vo.throw_skill_vo")
local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local damage = require("module.battle.skill.raw_skill_vo.damage_vo")
local effect = require("module.battle.skill.raw_skill_vo.effect_vo")
local property = require("module.battle.skill.raw_skill_vo.property_change_vo")
local buff = require("module.battle.skill.raw_skill_vo.buff_vo")
local calc = require("module.battle.skill.utils.caculate")
local decorator = require("module.battle.unit.behavior_tree.decorator")
local retarget = require("module.battle.skill.ripe_skill_vo.retarget_skill_vo")
local heal = require("module.battle.skill.raw_skill_vo.heal_vo")
local this = {}



local heal0 = heal.new()
heal0.calc = calc.make_common_calc(50) 

local normal0 = normal_skill.new()
normal0:append("raw_skills",heal0)

this.root = {normal0}
this.decorators = {decorator.check_hurt_friend_in_range(100,false)}
this.coold = 3

return this