local this = {}

function this.pack_database( caster,target,pos )
    local database = {
        caster = {
            unit = caster,
            attack = caster.property:get("attack"),
            defence = caster.property:get("defence"),
            magic_resist = caster.property:get("magic_resist"),
            crit = caster.property:get("crit"),
            crit_value = caster.property:get("crit_value"),
            hit_rate = caster.property:get("hit_rate"),
            dodge = caster.property:get("dodge")
        },
        target = {
            unit = target,
            attack = target.property:get("attack"),
            defence = target.property:get("defence"),
            magic_resist = target.property:get("magic_resist"),
            crit = target.property:get("crit"),
            crit_value = caster.property:get("crit_value"),
            hit_rate = caster.property:get("hit_rate"),
            dodge = target.property:get("dodge")
        },
        caster_pos = {
            X = pos.X,
            Y = pos.Y
        }
    }
    return database
end

return this