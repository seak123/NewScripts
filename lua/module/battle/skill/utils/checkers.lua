local math = math
local this = {}

function this.check_chance(chance)
	return function(sess,caster ,target) 
		return math.random() < chance 
	end
end


function this.check_attr( attr )
	return function ( sess,caster,target )
		if caster.attr == attr then return true
		else return false end
	end
end

function this.check_stack( num )
	return function ( sess,caster,target )
		local buff_trace = sess.trace:last_of_type("trace_buff")
		local buff = buff_trace.buff
		local max_num = num
	    if buff:get_inst_count() >= max_num then
            return true
		else
			return false
		end
	end
end

function this.check_ap( value )
	return function ( sess,caster,target )
		if target.ap <= value then
			return true
		else
			return false
		end
	end
end

function this.check_in_range(  )
    return function ( sess,database )
        local range = database.caster.unit.data.attack_range
        local radius1 = database.caster.unit.data.radius
        local radius2 = database.target.unit.data.radius
        local target_uid = database.target.unit.uid
        local target = database.target.unit
        if sess.field:distance(target,database.caster_pos)< ((radius1+radius2)*1.5 + range + 4) then
            return true
        else
            -- send miss icon
            return false
        end
    end
end


return this