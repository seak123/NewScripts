
-- stackable buff, common buff like poison and so on
local base = require("module.battle.unit.buff.base_buff")
local this = class("stack_buff", base)

function this:ctor(buff_id, buff_vo)
	self:init(buff_id, buff_vo)
	self.stacks = {}
	self.limit = self.config.stack 
end

function this:handle_stack(sess, inst)
	table.insert(self.stacks, inst)
	inst.carrier = self
	local count = #self.stacks 
	if count > self.limit then 
		table.sort(self.stacks , function(ins0, ins1) return ins0:get_turn_left() < ins1:get_turn_left() end)
		for n = 1, count - self.limit do
			table.remove(self.stacks)
		end
	end
  
  if self.trace == nil then 
    self.trace = {
      type_name = "trace_buff",
      caster = inst.caster,
      buff = self
    }
  end
  
	self:handle(sess, "on_change")
end

function this:update(sess, timing)
	-- update this stack
	if timing == self.config.timing then
		local prev = #self.stacks
		table.remove_if(self.stacks, function (ins) 
				ins:update(sess)
				return ins.is_expire
			end
		)
		local post = #self.stacks
		
		if post == 0 then 
			return self:remove(sess)
		elseif prev ~= post then 
			self:handle(sess, "on_update")
			return self:handle(sess, "on_change")
		else
			self:handle(sess, "on_update")
		end
	end
end

function this:check_live(sess)
	local flag = true
	 for _, v in ipairs(self.stacks) do
		flag = flag and v.is_expire
	 end
	 if false == true then return self:remove(sess) end
end

function this:get_inst_count() 
	return #self.stacks
end

function this:get_inst()
	local index = math.random(#self.stacks)
	return self.stacks[index]
end

function this:get_caster()
	local res = nil
	for _, v in ipairs(self.stacks) do
		if res == nil or res.timepass < v.timepass then 
			res = v 
		end
	end
	if res == nil then 
		return nil 
	else 
		return res.caster 
	end
end

function this:get_key()
	return self.buff_id
end

function this:get_turn_left()
  local res = 0
  for _, v in ipairs(self.stacks) do
    if v.duration <= 0 then 
      return -1 
    else
      res = math.max(res, v.duration - v.timepass)
    end
  end
  return res
end

function this:clear_inst( sess )
	self:remove(sess)
end


return this