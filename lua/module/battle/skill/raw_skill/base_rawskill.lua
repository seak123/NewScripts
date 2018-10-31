local this = class("rawskill")
--[[ sample ctor 
function this:ctor(vo)
	self:build_to_array("subs", vo.subs)
	
	-- for buff , etc
	self:build_to_array("on_add", vo.on_add)
end
]]


function this:check(sess,caster, target)
	if self.checkers ~= nil and #self.checkers > 0 then
		for _, v in ipairs(self.checkers) do
			if not v(sess, caster,target) then
				return false
			end
		end
	end
	return true
end

function this:execute_all(sess, caster, target)
end

function this:execute_one(sess, caster, target)
end

-- whether immune as simple effect
function this:immune(sess, caster, target)
	return false
end

function this:init_build( vo )
	self:build_to_array("subs",vo.subs)
	self.checkers = vo.checkers
end

function this:build_to_array(name, array)
	
	if array == nil then 
		return
	end
	local target = self[name]
	if target == nil then 
		target = {}
		self[name] = target
	end
	
	for _, v in ipairs(array) do 
		local temp = require(v.execute)
		if temp ~= nil then 
			local exec = temp.new(v)
			table.insert(target, exec)
		end
	end
end

function this:call_for_one(field, sess, caster, target)
	local arr = self[field]

	if arr ~= nil then 
		for _, v in ipairs(arr) do 
			v:execute_one(sess, caster, target,true)
		end
	end
end

function this:call_for_all(field, sess, caster, targets)
	local arr = self[field]
	
	if arr ~= nil then 
		for _, v in ipairs(arr) do 
			v:execute_all(sess, caster, targets,true)
		end
	end
end

function this:attach(buff)
  if self.vo == nil then 
    print(self.classname)
  end
	local occasion = self.vo.buff_occasion
	if occasion ~= nil then 
		buff:append_callback(occasion, self.execute_one, self)
	end 
end

function this:detach(buff)
	local occasion = self.vo.buff_occasion
	if occasion ~= nil then 
		buff:remove_callback(occasion, self.execute_one, self)
	end 
end

function this.uname(caster)

end

return this