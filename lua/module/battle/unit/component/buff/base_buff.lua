local this = class("base_buff")
local bit = require("utils.bit_calc")

function this:init( sess,buff_id,buff_vo )
	self.sess = sess
    self.buff_id = buff_id
    self.vo = buff_vo
    self.is_remove = false
    self.feature = self.vo.feature

end

function this:attach_buff(sess, unit)
	self.carrier = unit
	self.is_remove = false
end

function this:detach_buff(sess, unit)

	self.carrier = nil
end

function this:handle(sess, name)
	local ev_name = "ev_"..name
	local ev = self[ev_name]
	if ev ~= nil then 
		for _, v in ipairs(ev) do
			v.func(v.obj,sess,self.carrier)
		end
	end
end

function this:append_callback(name, func, obj)
	local ev_name = "ev_"..name
	local ev = self[ev_name]
	if ev == nil then 
		self[ev_name] = {}
	end
  table.insert(self[ev_name],{func =func,obj =obj} )
end

function this:remove_callback(name, func, obj)
	local ev_name = "ev_"..name
	local ev = self[ev_name]
	if ev ~= nil then 
		for i,v in ipairs(ev) do
			if v.func == func and v.obj == obj then
				table.remove( ev, i )
				return
			end
		end
	end
end

function this:remove(sess)
	self:handle(sess, "on_remove")
	-- remove , inform view now

	self.is_remove = true
end

function this:has_feature(value)
  return bit._and(self.feature, value) == value
end

function this:non_feature(value)
  return bit._and(self.feature, value) == 0
end

function this:one_feature(value)
	if value == 0 then return true end
  return bit._and(self.feature, value) ~= 0
end

return this
