
-- stackable buff, common buff like poison and so on
local base = require("module.battle.unit.component.buff.base_buff")
local this = class("stack_buff", base)

function this:ctor(sess,buff_id, buff_vo)
	self:init(sess,buff_id, buff_vo)
	self.stacks = {}
	self.limit = buff_vo.max_stack
end

function this:handle_stack (sess,inst)
	table.insert(self.stacks, inst)
	inst.buff = self
	local count = #self.stacks 
	if count > self.limit then 
		for n = 1, count - self.limit do
			print("@@detach2")
			self:remove_stack(1)
		end
	end
	inst:attach(sess)
end

function this:remove_stack( index )
	print("@@detach1")
	 self.stacks[index]:detach(self.sess)
	 table.remove( self.stacks, index)
end

function this:update(delta)
	print("@@stack update")
	for index=#self.stacks,1,-1 do
		self.stacks[index]:update(delta)
		if self.stacks[index].is_expire == true then
			print("@@detach3")
			self:remove_stack(index)
		end
	end
	self:check_live()
end

function this:check_live()
	if #self.stacks == 0 then self.is_remove = true end
end

function this:get_inst_count() 
	return #self.stacks
end

function this:get_inst()
	local index = math.random(#self.stacks)
	return self.stacks[index]
end

function this:get_key()
	return self.buff_id
end

function this:remove(sess)

	if #self.stacks > 0 then
		for index =#self.stacks,1,-1 do
			print("@@detach4")
			self:remove_stack(index)
		end
	end

	self.is_remove = true
end


return this