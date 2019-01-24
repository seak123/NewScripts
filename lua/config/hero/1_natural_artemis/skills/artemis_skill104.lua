local throw_skill = require("module.battle.skill.ripe_skill_vo.throw_skill_vo")
local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local effect = require("module.battle.skill.raw_skill_vo.effect_vo")
local decorator = require("module.battle.unit.behavior_tree.decorator")
local summon = require("module.battle.skill.raw_skill_vo.summon_vo")
local pack = require("module.battle.skill.utils.pack_database")
local this = {}

local summon0 = summon.new()
summon0.live_time = 40
summon0.data =pack.get_arg(false,20001)
summon0.num =pack.get_arg(false,3)

local summon1 = summon.new()
summon1.live_time = 40
summon1.data =pack.get_arg(false,20002)
summon1.num =pack.get_arg(false,1)

local normal = normal_skill.new()
normal:append("raw_skills",summon0,summon1)

this.root = {normal}
this.decorators = {}
this.coold = 60

return this