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

-- whether immune as simple effect
function this:immune(sess, caster, target)
	return false
end

function this:init_build( vo )
	self:build_to_array("subs",vo.subs)
	self.checkers = vo.checkers
end

function this:execute_subs( sess,target )
	if self.subs ~= nil then
		for _, v in ipairs(self.subs) do
			v:execute(sess,target)
		end
	end
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
			local exec = temp.new(v,self.database)
			table.insert(target, exec)
		end
	end
end


function this:attach(buff)
  if self.vo == nil then 
    --print(self.classname)
  end
	local occasion = self.vo.buff_occasion
	if occasion ~= nil then 
		buff:append_callback(occasion, self.execute, self)
	end 
end

function this:detach(buff)
	local occasion = self.vo.buff_occasion
	if occasion ~= nil then 
		buff:remove_callback(occasion, self.execute, self)
	end 
end

function this.uname(caster)

end

return this