local throw_skill = require("module.battle.skill.ripe_skill_vo.throw_skill_vo")
local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local effect = require("module.battle.skill.raw_skill_vo.effect_vo")
local decorator = require("module.battle.unit.behavior_tree.decorator")
local summon = require("module.battle.skill.raw_skill_vo.summon_vo")
local pack = require("module.battle.skill.utils.pack_database")
local this = {}




local effect0 = effect.new()
effect0.effect_id = 2
effect0.execute_pos = effect.ExecutePos.Caster
effect0.attach = true

local summon0 = summon.new()
summon0.data =pack.get_arg(false,3)
summon0.num =pack.get_arg(false,1)


local normal = normal_skill.new()
normal:append("raw_skills",summon0)

this.root = {normal}
this.decorators = {decorator.check_summon()}
this.coold = 10

return this