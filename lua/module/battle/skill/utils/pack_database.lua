local this = {}
local battle_def = require("module.battle.battle_def")

function this.pack_database( _caster,_target,pos )
    local database = {
        caster = _caster,

        target = _target,
        caster_pos = {
            X = _caster.transform.grid_pos.X,
            Y = _caster.transform.grid_pos.Y
        },
        target_pos = {
            X = pos.X,
            Y = pos.Y
        },
        target_trace= {

        }
    }
    return database
end

function this.pack_common_database( sess,side,pos,arg1,arg2,arg3 )
    local database = {
        caster = {
            unit = sess.players[1].unit,
            attack = sess.players[1].unit.property:get("attack"),
            defence = sess.players[1].unit.property:get("defence"),
            magic_resist = sess.players[1].unit.property:get("magic_resist")
        },
        caster_pos = {
            X = battle_def["PLAYER"..side.."POS"].X,
            Y = battle_def["PLAYER"..side.."POS"].Y    
        },
        target = {
            unit = nil
        },
        target_pos = {
            X = pos.X,
            Y = pos.Y
        },
        args = {
            [1]=arg1,
            [2]=arg2,
            [3]=arg3,
        },
        target_trace= {
            
        }
    }
    if side == 2 then
        database.caster = {
            unit = {side = 2},
            attack = 1,
            defence = 1,
            magic_resist = 0.5,
        }
    end
    return database
end

function this.get_arg( need_get,index )
    return function (database  )
        if need_get == true then
            return database.args[index]
        else
            return index
        end
    end
end
return this