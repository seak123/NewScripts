local base = require("module.battle.skill.raw_skill.base_rawskill")
local this = class("heal",base)
local calc = require("module.battle.skill.utils.caculate")
local trace = require("module.battle.battle_trace")

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

    local heal_value = calc.heal(database.caster, target, value)

    -- push trace
    local trace_heal = trace.trace_heal(heal_value,database.caster,target)
    sess.trace:push(trace_heal)

    database.caster:pre_heal()
    target:pre_healed()

    --logic
    target:heal(trace_heal.heal_value, database.caster)

    database.caster:post_heal()
    target:post_healed()

end

return this