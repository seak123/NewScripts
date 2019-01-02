local this = class("state_control")

-- feature: de_move,de_attack,de_skill,taunt,confused,immune
this.state_feature = {
    frozen = {"de_move","de_attack"},
    cold = {"de_speed","de_attack_rate"},
	stun = {"de_move","de_attack","de_skill"},
	taunt = {"taunt"},
    confused = {"confused"},
    silence = {"de_skill"},
    immune = {"immune"},
    unarm = {"de_attack"},
    demove = {"de_move"}
}

function this.contains(state_name,feature)
	local features = this.state_feature[state_name]
	if features ~= nil then
	    for _,v in ipairs(features) do
		    if v == feature then return true end
	    end
    end
	return false
end


function this:ctor(master)
	-- body
	self.master = master

    -- state name
    self.cold = 0
    self.frozen = 0
    self.stun = 0
    self.taunt = 0
    self.confused = 0
    self.silence = 0
    self.immune = 0
    self.unarm = 0
    self.demove = 0
    -- -- feature name
    -- self.de_move = 0
    -- self.de_attack = 0
    -- self.de_skill = 0
    -- self.taunt = 0
    -- self.confused = 0
    -- self.immune = 0
    
    -- state prop
    self.taunt_target = nil
end

function this:has_feature(feature)
    local value = self["feature_"..feature]
    return value and value > 0
end


function this:state_get(sess, state_name)
    
    -- structure cannot has state change
    if self.master.type == 2 then return end

	local state = self[state_name]

	local new_state = state + 1
	
	self[state_name] = new_state
	local feature = this.state_feature[state_name]
	
	if feature ~= nil then 
		for _, v in ipairs(feature) do
			local fname = "feature_"..v
			if self[fname] == nil then self[fname] = 0 end 
            self[fname] = self[fname] + 1
            if self[fname] == 1 then
                self[v.."_get"](self,sess)
            end
		end
	end
	
    if new_state == 1 then
        self["notify_"..state_name.."_get"](self,sess)
	end
end

function this:state_lose(sess, state_name)

     -- structure cannot has state change
     if self.master.type == 2 then return end

	
	local state = self[state_name]
	local new_state = 0
	if state > 0 then 
        local new_state = state - 1
    end
    
    self[state_name] = new_state
    local feature = this.state_feature[state_name]
    
	if feature ~= nil then 
		for _, v in ipairs(feature) do
            local fname = "feature_"..v
            if self[fname] >0 then
                self[fname] = self[fname] - 1
            end
            if self[fname] == 0 then
                self[v.."_lose"](self,sess)
            end
		end
	end
    
    if new_state == 0 then
        self["notify_"..state_name.."_lose"](self,sess)
    end
	
end


-- frozen,stun,taunt,confused,silence,immune,unarm,demove
------------------------------state control
function this:notify_cold_get(  )
    self.master.entity:SetColor("blue");
end

function this:notify_cold_lose(  )
    self.master.entity:RemoveColor("blue");
end

function this:notify_frozen_get(  )
    -- body
end

function this:notify_frozen_lose(  )
    -- body
end

function this:notify_stun_get(  )
    -- body
end

function this:notify_stun_lose(  )
    -- body
end

function this:notify_taunt_get(  )
    -- body
end

function this:notify_taunt_lose(  )
    -- body
end

function this:notify_confused_get(  )
    -- body
end

function this:notify_confused_lose(  )
    -- body
end

function this:notify_silence_get(  )
    -- body
end

function this:notify_silence_lose(  )
    -- body
end

function this:notify_immune_get(  )
    -- body
end

function this:notify_immune_lose(  )
    -- body
end

function this:notify_unarm_get(  )
    -- body
end

function this:notify_unarm_lose(  )
    -- body
end

function this:notify_demove_get(  )
    -- body
end

function this:notify_demove_lose(  )
    -- body
end

-- feature: de_move,de_attack,de_skill,taunt,confused,immune
------------------------------feature control

function this:de_move_get(  )
    -- body
end

function this:de_move_lose(  )
    -- body
end

function this:de_attack_get(  )
    -- body
end

function this:de_attack_lose(  )
    -- body
end

function this:de_skill_get(  )
    -- body
end

function this:de_skill_lose(  )
    -- body
end

function this:taunt_get( sess )
    local taunt_target
    local target_uid = sess.trace:get_last_type("trace_state").caster_uid
    self.taunt_target = sess.field:get_unit(target_uid)
end

function this:taunt_lose( sess )
    -- body
end

function this:confused_get(  )
    -- body
end

function this:confused_lose(  )
    -- body
end

function this:immune_get(  )
    -- body
end

function this:immune_lose(  )
    -- body
end

function this:de_speed_get(  )
    -- body
    self.master.property:change_prop("speedrate",-0.5)
end

function this:de_speed_lose(  )
    -- body
    self.master.property:change_prop("speedrate",0.5)
end

function this:de_attack_rate_get(  )

    self.master.property:change_prop("attack_rateadd",-1)

end

function this:de_attack_rate_lose(  )
    -- body
    self.master.property:change_prop("attack_rateadd",1)
end

return this