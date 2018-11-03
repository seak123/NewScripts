local this = {}

function this.pack_database( _caster,_target,pos )
    local database = {
        caster = {
            unit = _caster,
            attack = _caster.property:get("attack"),
            defence = _caster.property:get("defence"),
            magic_resist = _caster.property:get("magic_resist"),
            crit = _caster.property:get("crit"),
            crit_value = _caster.property:get("crit_value"),
            hit_rate = _caster.property:get("hit_rate"),
            dodge = _caster.property:get("dodge")
        },
        target = {
            unit = _target
            -- attack = target.property:get("attack"),
            -- defence = target.property:get("defence"),
            -- magic_resist = target.property:get("magic_resist"),
            -- crit = target.property:get("crit"),
            -- crit_value = caster.property:get("crit_value"),
            -- hit_rate = caster.property:get("hit_rate"),
            -- dodge = target.property:get("dodge")
        },
        caster_pos = {
            X = _caster.transform.grid_pos.X,
            Y = _caster.transform.grid_pos.Y
        },
        target_pos = {
            X = pos.X,
            Y = pos.Y
        }
    }
    return database
end

return this