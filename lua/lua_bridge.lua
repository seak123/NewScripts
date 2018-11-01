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

---------------------------lua function

function lua_update( delta )
    root.session:update(delta)
end

function add_unit( data)
    root.session.field:add_unit(data)
end

---------------------------csharp functon

-- function CreateEntity( unitid )
--     return api.CreateEntity(unitid)
-- end

--MapField,Entity,Particle

function GetMapField(  )
    return api.GetMapField()
end

function GetEffectManager(  )
    --return api.GetEffectManager()
end


