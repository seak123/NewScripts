
local this = class("ai_tree")



function this:build( player_data,vo )
    -- init database
    local database = self:build_database(player_data)
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

function this:build_database( )
    local data = {}
    data.sess = battle_mng.session
    data.main_castle = data.sess.players[2].unit
    data.cards = data.sess.players[2].cards
    return data
end



return this