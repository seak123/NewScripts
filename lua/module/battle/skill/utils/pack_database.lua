local this = {}

function this.pack_database( caster,target,pos )
    local database = {
        caster = {
            unit = caster,
            attack = caster.property:get("attack")
        },
        target = {
            unit = target
        },
        caster_pos = {
            X = pos.X,
            Y = pos.Y
        }
    }
    return database
end

return this