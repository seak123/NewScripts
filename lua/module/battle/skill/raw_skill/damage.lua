local base = require("module.battle.skill.row_skill.base_rowskill")
local this = class("damage",base)

function this:ctor( vo )
    self.vo = vo
    self:init_build(vo)
end

function this:execute(sess, delta ,database)
    if self:check(sess,database) == false then
        return true
    end
    local damage_value = self.calc()

    local judge,damage = calc.damage(caster, target, total_atk, self.vo.damage_type)
    damage = damage * self.vo.damage_mult
    
    target:damage(trace_dmg, caster)

    return true
end

return this