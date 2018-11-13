local this = {}
local battle_def = require("module.battle.battle_def")

function this.pack_database( _caster,_target,pos )
    local database = {
        caster = {
            unit = _caster,
            attack = _caster.property:get("attack"),
            defence = _caster.property:get("defence"),
            magic_resist = _caster.property:get("magic_resist"),
            crit = _caster.property:get("crit"),
            crit_value = _caster.property:get("crit_value"),
            hit_rate = _caster.property:get("hit_rate"),
            dodge = _caster.property:get("dodge")
        },
        target = {
            unit = _target
            -- attack = target.property:get("attack"),
            -- defence = target.property:get("defence"),
            -- magic_resist = target.property:get("magic_resist"),
            -- crit = target.property:get("crit"),
            -- crit_value = caster.property:get("crit_value"),
            -- hit_rate = caster.property:get("hit_rate"),
            -- dodge = target.property:get("dodge")
        },
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

function this.pack_common_database( sess,side,pos,arg1,arg2 )
    local database = {
        caster = {
            unit = sess.players[side],
            attack = sess.players[side].attack,
            defence = sess.players[side].defence,
            magic_resist = sess.players[side].magic_resist
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
            [2]=arg2
        },
        target_trace= {
            
        }
    }
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