
local this = class("ai_tree")



function this:build( sess,vo )
    -- init database
    local database = self:build_database(sess)
    -- start build tree
    local queue = {}
    
    local root_node = require(vo.execute).new(vo,database)
   

    table.insert( queue,root_node)
    while #queue > 0 do
        local node_vo = queue[1].vo
        local node = queue[1]
        if node_vo.subs ~= nil then
            for _,v in ipairs(node_vo.subs) do
                local sub_node = require(v.execute).new(v,database)
                table.insert(node.childs,sub_node)
                table.insert( queue, sub_node)
            end
        end
        node:init()
        table.remove( queue, 1 )
    end
    return root_node
end

function this:build_database( sess)
    local data = {}
    data.sess = sess
    data.main_castle = data.sess.players[2].unit
    return data
end



return this