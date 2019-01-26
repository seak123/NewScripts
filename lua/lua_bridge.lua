
require("utils.functions")
battle_mng = require("module.battle.battle_manager")
config_mng = require("config.config_manager")
local api = Utils.Lua2CSharpAPI

root = {mng = battle_mng}
local time = 0
local flag = 0

print("lua_bridge start")

function lua_init(  )
    print("lua init ...")
    return root
end

---------------------------lua function
function start_battle( battle_vo )
    math.randomseed(os.time())
    battle_mng.battle_begin(battle_vo)
end

function lua_update( delta )
    if battle_mng.session ~= nil then
        battle_mng.session:update(delta)
    end
end

function caster_skill( side,skill_id,pos_x,pos_y,arg1,arg2)
    if battle_mng.session ~= nil then
        battle_mng.session.skill_mng:caster_skill(side,skill_id,pos_x,pos_y,arg1,arg2)
    end
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
    return api.GetEffectManager()
end

function GetPlayerManager(  )
    return api.GetPlayerManager()
end

function GetAssetManager()
    return api.GetAssetManager()
end

function BattleCompleted( res )
    return api.BattleCompleted(res)
end

function GetUnitData( id )
    return api.GetUnitData(id)
end




