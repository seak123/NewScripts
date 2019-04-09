local this = class("base_trigger")


-- this.checkers
function this:ctor(  )
	--self.target_type = "self"
end

-- source return: 1,==0,target's uid 2, <0 room id 3,<-100 others
function this:get_source() 
  if self.source > 0 then 
    return self.source
  elseif self.source == 0 then
    return self.owner.uid
  elseif self.source == -1 then
    return -self.owner.location
  elseif self.source == -2 then
    -- for other use
    return self.owner.side - 3
  else
    error("invalid source type")
  end
end

--keep checkers
function this:check(sess, event_name, target)
	if self.vo.occasion == event_name then 
    -- if self.checkers ~= nil then
    --   for _, v in ipairs(self.checkers) do
    --     if not v(sess, self,target) then 
    --       return false
    --     end
    --   end
    -- end
		return true
	else 
		return false
	end
end

function this:execute(trigger,sess, target)

end


return this