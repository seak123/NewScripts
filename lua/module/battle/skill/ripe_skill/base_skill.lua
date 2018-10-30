local this = class("base_skill")

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

return this