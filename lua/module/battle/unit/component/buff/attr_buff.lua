
-- attribute buff, common buff like rage and so on
local base = require("module.battle.unit.component.buff.base_buff")
local this = class("attr_buff", base)

function this:ctor(sess,buff_id, buff_vo,database)
	self:init(sess,buff_id, buff_vo)
	self.sess = sess
	self.stack = 0
	self.limit = buff_vo.max_stack
	self.update_type = buff_vo.update_type
	self.belongs = {}

    if buff_vo.belongs ~= nil then
        for _,v in ipairs(buff_vo.belongs) do
            local clazz = require(v.execute)
            local item = clazz.new(v,database)
            table.insert( self.belongs, item)
        end
    end
	
	for _, v in ipairs(self.belongs) do
		v:attach(self)
	end

	self:append_callback(buff_vo.tick_occasion, this.remove_stack, self)
end

function this:handle_stack (sess,inst,stack_num)
	self.stack = self.stack + stack_num

	for _, v in ipairs(self.belongs) do
        if v.vo.buff_occasion == "on_add" then
            v:execute(sess,self.carrier)
        end
	end
end

function this:remove_stack()
	self.stack = math.floor( self.stack/2 )
end

function this:update(delta)
    for _,v in ipairs(self.belongs) do
		if v.vo.buff_occasion == "on_tick" then
			v:execute(self.sess,self.carrier)
        end
	end
	
	self:check_live()
end

function this:check_live()
	if self:get_inst_count() == 0 then self.is_remove = true print("@@is_removeing!!!!!") end
end

function this:get_inst_count() 
	return self.stack
end


function this:get_key()
	return self.buff_id
end

function this:remove(sess)
	for _, v in ipairs(self.belongs) do
        if v.vo.buff_occasion == "on_remove" then
            v:execute(sess,self.buff.carrier)
        end
		v:detach(self)
	end
	self.is_remove = true
end


return this