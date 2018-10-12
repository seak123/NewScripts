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
--    if flag == 0 and time > 5 then
--     flag = 1
--     print("lua: Add unit 1")
--     --if api == nil then print("api is nil") end
--     --api.AddUnit(0,6,10)
--     --print("lua: Add unit 1")
--     --print(tostring(api.GetBattleField()))
--    end
end

function add_unit( data )
    print(tostring(data))
    for n,v in pairs(data) do
        --print("data name:"..tostring(n).." value:"..tostring(v))
    end
end


