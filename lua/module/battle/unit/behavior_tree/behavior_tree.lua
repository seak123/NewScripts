
local this = class("behavior_tree")



function this:build( vo )
    local queue = {}
    local root_node = require(vo.execute).new(vo)
    table.insert( queue,root_node)
    while #queue > 0 do
        local node_vo = queue[1].vo
        local node = queue[1]
        if node_vo.subs ~= nil then
            for _,v in ipairs(node_vo.subs) do
                local sub_node = require(v.execute).new(v)
                table.insert(node.childs,sub_node)
                table.insert( queue, sub_node)
            end
        end
        table.remove( queue, 0 )
    end
    return root_node
end



return this