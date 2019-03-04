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

    local judge,damage = calc.damage(database.caster, target, value,self.vo.damage_source ,self.vo.damage_type)

    -- push trace
    local trace_damage = trace.trace_damage(damage,database.caster,target)
    sess.trace:push(trace_damage)

    if self.vo.damage_source == damage_vo.DamageSource.Attack then
        database.caster:pre_normal_damage()
        target.pre_normal_damaged()
    end

    database.caster:pre_damage()
    target:pre_damaged()

    --logic
    target:damage(trace_damage.damage_value, database.caster)
    self:life_steal(sess,database,trace_damage)

    database.caster:post_damage()
    target:post_damaged()

end

function this:life_steal( sess,database,trace_data )
    if database.caster.alive ~= 0 then return end
    local rate = 0
    if self.vo.damage_type == damage_vo.DamageType.Physical then
        rate = database.caster.property:get("physic_suck")
    elseif self.vo.damage_type == damage_vo.DamageType.Magic then
        rate = database.caster.property:get("magic_suck")
    end
    if rate == 0 then return end
    local heal_value = trace_data.damage_value*rate
    local trace_heal = trace.trace_heal(heal_value,database.caster,database.target)
    sess.trace:push(trace_heal)

    database.caster:pre_healed()
    database.caster:heal(heal_value,database.caster)
    database.caster:post_healed()
end

return this