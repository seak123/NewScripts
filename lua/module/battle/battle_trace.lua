local this = class("battle_trace")

function this:ctor( sess )
    self.sess = sess
    self.last = {}
end

function this:push( trace_data )
    self.last[trace_data.type_name] = trace_data
end

function this:get_last_type( type_name )
    local trace_data = self.last[type_name]
    return trace_data
end

function this.trace_damage(value,caster,target )
    return {
        type_name = "trace_damage",
        damage_value = value,
        caster_uid = caster.uid,
        target_uid = target.uid
    }
end

function this.trace_heal( value,caster,target )
    return {
        type_name = "trace_heal",
        heal_value = value,
        caster_uid = caster.uid,
        target_uid = target.uid
    }
end

return this