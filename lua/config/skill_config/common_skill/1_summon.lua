local throw_skill = require("module.battle.skill.ripe_skill_vo.throw_skill_vo")
local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local summon = require("module.battle.skill.raw_skill_vo.summon_vo")
local effect = require("module.battle.skill.raw_skill_vo.effect_vo")
local calc = require("module.battle.skill.utils.caculate")
local pack = require("module.battle.skill.utils.pack_database")
local this = {}

local summon0 = summon.new()
summon0.data =pack.get_arg(true,1)
summon0.num =pack.get_arg(true,2)
summon0.struct_uid = pack.get_arg(true,3)

local normal = normal_skill.new()
normal:append("raw_skills",summon0)

this.root = {normal}

return this