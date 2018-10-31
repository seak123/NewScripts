local base = require("module.battle.skill.raw_skill.base_rawskill")
local this = class("damage",base)
local calc = require("module.battle.skill.utils.caculate")

function this:ctor( vo )
    self.vo = vo
    self:init_build(vo)
end

function this:execute(sess, delta ,database)
    if self:check(sess,database) == false then
        return true
    end
    local damage_value = self.vo.calc(sess,database.caster,database.target)

    local judge,damage = calc.damage(database.caster, database.target, damage_value, self.vo.damage_type)
    
    database.target.unit:damage(damage, database.caster.unit)

    return true
end

return this