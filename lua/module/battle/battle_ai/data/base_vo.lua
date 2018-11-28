
local this = class("base_vo")

function this:append( name,... )
    local args = {...}
    local target = rawget(self,name)
    if target == nil then
        self[name] = args
    else
        local len = #target
        for k,v in ipairs(args) do
            target[k+len] = v
        end
    end
end


return this