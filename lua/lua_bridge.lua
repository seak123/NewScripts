
require("utils.functions")
local battle_mng = require("module.battle.battle_manager")
local api = Utils.Lua2CSharpAPI

root = {mng = battle_mng}
local time = 0
local flag = 0
function lua_init(  )
    print("lua init ...")
    return root
end

---------------------------lua function
function start_battle( battle_vo )
    battle_mng.battle_begin(battle_vo)
end

function lua_update( delta )
    if battle_mng.session ~= nil then
        battle_mng.session:update(delta)
    end
end

function caster_skill( side,skill_id,pos_x,pos_y,arg1,arg2)
    battle_mng.session.skill_mng:caster_skill(side,skill_id,pos_x,pos_y,arg1,arg2)
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

function BattleCompleted(  )
    return api.BattleCompleted()
end


