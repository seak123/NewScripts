local math = math
local this = {}

function this.check_chance(chance)
	return function(sess,database ,target) 
		return math.random() < chance 
	end
end


function this.check_in_range(  )
    return function ( sess,database ,target)
        local range = database.caster.data.attack_range
        local radius1 = database.caster.data.radius
        local radius2 = target.data.radius
        local target_uid = target.uid
        if sess.field:distance(target,database.caster_pos)< ((radius1+radius2)*1.5 + range + 4) then
            return true
        else
            -- send miss icon
            return false
        end
    end
end

function this.check_attack_range( is_short )
	return function ( sess,database,target )
		if is_short then
			return target.data.attack_range < 40
		else
			return target.data.attack_range >= 40
		end
	end
end


return this