local fixed_skill = require("module.battle.skill.fixed_skill")
local damage = require("module.battle.skill.row_skill_vo.damage_vo")
local root = {}



local damage0 = damage.new()

local fixed_skill0 = fixed_skill.new()
fixed_skill0:append("subs",damage0)


root.childs = {fixed_skill0}

return root