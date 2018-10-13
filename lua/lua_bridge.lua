root = {}
local sample = require("test.sample")
local api = Utils.Lua2CSharpAPI

local time = 0
local flag = 0
function lua_init(  )
    print("lua init ...")
    sample.init(root)
    return root
end

function lua_update( delta )
    root.session:update(delta)
end

function add_unit( data)
    root.session.field:add_unit(data)
end


