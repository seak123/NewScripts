local normal_skill = require("module.battle.skill.ripe_skill_vo.normal_skill_vo")
local damage = require("module.battle.skill.row_skill_vo.damage_vo")
local calc = require("module.battle.skill.utils.caculate")
local check = require("module.battle.skill.utils.checkers")


local damage0 = damage.new()
damage0.calc = calc.make_common_attack(1, 0) 
damage0.checkers = {check.check_in_range}
  

local fixed_skill0 = normal_skill.new()
fixed_skill0:append("raw_skills",damage0)


local root = {fixed_skill0}

return root