local this = class("base_buff")

function this:init( buff_id,buff_vo )
    self.buff_id = buff_id
    self.vo = buff_vo
    self.is_remove = false
    self.belongs = {}
    self.feature = self.vo.feature
    if buff_vo.belongs ~= nil then
        for _,v in ipairs(buff_vo.belongs) do
            local clazz = require(v.execute)
            local item = clazz.new(v)
            table.insert( self.belongs, item)
        end
    end
end

function this:attach(sess, unit)
	self.carrier = unit
	-- reset is_remove beacuse buff maybe re-attached (steal buff)
	self.is_remove = false
	for _, v in ipairs(self.belongs) do
		v:attach(self)
	end
end

function this:detach(sess, unit)
	for _, v in ipairs(self.belongs) do
		v:detach(self)
	end
	self.carrier = nil
end

function this:handle(sess, name)
	local ev_name = "ev_"..name
	local ev = self[ev_name]
	if ev ~= nil then 
    local caster = self:get_caster()
		print(self.carrier.name.." dispatch ["..name.."] in buffcont") 
		self:push_buff(sess)
		ev(sess, caster, self.carrier)
		self:pop_buff(sess)
	end
end

function this:push_buff(sess)
    local trace = sess.trace
    trace:push(self.trace)
	  --[[if self.trace_skill ~= nil then 
		trace:push(self.trace_skill)
	  end
  
	  trace:push(self.trace_buff)]]

end

function this:pop_buff(sess)
    local trace = sess.trace
    trace:pop()
	 --[[ if self.trace_skill ~= nil then 
		trace:pop()
    end
    trace:pop()]]
end
 
function this:append_callback(name, func, obj)
	local ev_name = "ev_"..name
	local ev = self[ev_name]
	if ev == nil then 
		ev = event(ev_name, true)
		self[ev_name] = ev
	end
  ev:add(func, obj)
end

function this:remove_callback(name, func, obj)
	local ev_name = "ev_"..name
	local ev = self[ev_name]
	if ev ~= nil then 
		ev:remove(func, obj)
	end
end

function this:remove(sess, is_move)
	self:handle(sess, "on_remove")
	-- remove , inform view now
  sess.logger:log("[BUFF] buff "..self.vo.buff_id.."_"..self.config.name.." removed from "..self.carrier:full_name()) 
	self.is_remove = not is_move
end

function this:has_feature(value)
  return bit.band(self.feature, value) == value
end

function this:non_feature(value)
  return bit.band(self.feature, value) == 0
end

function this:one_feature(value)
	if value == 0 then return true end
  return bit.band(self.feature, value) ~= 0
end

return this
--[[ implemented by child type
function this:handle_stack(sess, inst)
end

function this:update(sess)
end

function this:get_stack()

function this:get_caster()
]]
