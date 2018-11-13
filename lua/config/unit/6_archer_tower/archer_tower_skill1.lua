local throw_skill = require("module.battle.skill.ripe_skill_vo.throw_skill_vo")
local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local damage = require("module.battle.skill.raw_skill_vo.damage_vo")
local effect = require("module.battle.skill.raw_skill_vo.effect_vo")
local property = require("module.battle.skill.raw_skill_vo.property_change_vo")
local buff = require("module.battle.skill.raw_skill_vo.buff_vo")
local calc = require("module.battle.skill.utils.caculate")
local decorator = require("module.battle.unit.behavior_tree.decorator")
local this = {}



local damage0 = damage.new()
damage0.calc = calc.make_common_attack(1, 0) 

local normal = normal_skill.new()
normal:append("raw_skills",damage0)

local effect0 = effect.new()
effect0.effect_id = 1

local throw = throw_skill.new()
throw.speed = 50
throw:append("effect",effect0)
throw:append("childs",normal)


this.root = {throw}
this.decorators = {decorator.check_skill_EnemyInRange(100,true)}
this.coold = 0.5

return this