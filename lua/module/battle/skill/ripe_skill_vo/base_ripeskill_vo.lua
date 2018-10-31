
local this = class("base_ripeskill_vo")

function this:append(field, ...)
	local arr = {...}
	local target = rawget(self, field)
	if target == nil then 
		self[field] = arr
	else 
		local len = #target
		for k, v in ipairs(arr) do
			target[len + k] = v
		end
	end
end

return this