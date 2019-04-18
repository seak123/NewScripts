
local base = require("module.battle.skill.raw_skill_vo.base_rawskill_vo")
local this = class("buff_vo",base)

this.execute = "module.battle.skill.raw_skill.buff"

this.BuffType = {
    Stack = 1,
    Attr = 2,
}

this.buff_id = 0
this.duration = 0
-- max_stack == -1 means no limit
this.max_stack = 1
this.stack_num = 1

-- 0: instance 1:all-refresh
this.update_type = 0

--feature bit means:  0-benefit(0no1yes);2-dispel(0cannot1can);
this.feature = 0
this.belongs = {}

-- 0:caster 1:target
this.execute_type = 1

-- buff type: 1,stack  2,attr:update after attack
this.buff_type = this.BuffType.Stack


----------attr buff property
this.tick_occasion = "post_attack"

return this