
local this = class("base_node_vo")

function this:append( name,... )
    local args = ...
    if self[name] == nil then
        self[name] = {}
    end
    for _,v in ipairs(args) do
        table.insert( self[name], v)
    end
end


return this