local base = require("module.battle.skill.raw_skill.base_rawskill")
local this = class("damage",base)
local calc = require("module.battle.skill.utils.caculate")
local trace = require("module.battle.battle_trace")
local damage_vo = require("module.battle.skill.raw_skill_vo.damage_vo")

function this:ctor( vo,database )
    self.vo = vo
    self.database = database
    self:init_build(vo)
end

function this:execute(sess, target)
    -- base execute
    if target.alive ~= 0 then return end
    if self:check(sess,self.database,target) == false then
        return
    end

    local database = self.database
    -- damage execute
    local value = self.vo.calc(sess,database.caster,target)

    local judge,damage = calc.damage(database.caster, target, value, self.vo.damage_type)

    -- push trace
    local trace_damage = trace.trace_damage(damage,database.caster.unit,target)
    sess.trace:push(trace_damage)

    database.caster.unit:pre_damage()
    target:pre_damaged()

    --logic
    target:damage(trace_damage.damage_value, database.caster.unit)
    self:life_steal(sess,database,trace_damage)

    database.caster.unit:post_damage()
    target:post_damaged()

end

function this:life_steal( sess,database,trace_data )
    if database.caster.unit.alive ~= 0 then return end
    local rate = 0
    if self.vo.damage_type == damage_vo.DamageType.Physical then
        rate = database.caster.unit.property:get("physic_suck")
    elseif self.vo.damage_type == damage_vo.DamageType.Magic then
        rate = database.caster.unit.property:get("magic_suck")
    end
    if rate == 0 then return end
    local heal_value = trace_data.damage_value*rate
    local trace_heal = trace.trace_heal(heal_value,database.caster.unit,database.target.unit)
    sess.trace:push(trace_heal)

    database.caster.unit:pre_healed()
    database.caster.unit:heal(heal_value,database.caster.unit)
    database.caster.unit:post_healed()
end

return this