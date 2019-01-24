local this = class("base_rawskill_vo")

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

function this:collect_assets(arr)
	-- body
	if self.collect_self ~= nil then 
		self:collect_self(arr)
	end
	return self:collect_array(self.subs, arr)
end

function this:collect_array(childs, target)
	-- body
	if childs ~= nil then 
		for _, v in ipairs(childs) do 
			v:collect_assets(target)
		end
	end
end

this.checkers = {}

this.interval = 0.1
this.buff_occasion = "on_add"

return this