local this = class("ai_decorator")
local battle_def = require("module.battle.battle_def")

function this:ctor( vo )
    self.type = vo.type
end

function this:init_data( database )
    self.database = database
end

-- this.Type = {
--     CalcPriority = "CalcPriority",
--     CardAvaliable = "CardAvaliable",
--     CalcData = "CalcData"
-- }


function this:check(  )
    return self["check_"..self.type](self)
end

function this:check_CalcPriority(  )
    local card_list = {}
    local cards = GetPlayerManager().enemyCards
    self.database.play_id = -1
    if cards == nil or cards.Length == 0 then return true end
    for i=0,cards.Length-1 do
        if cards[i] >= 0 then
            local value = 0
            local card_config = config_mng.get_card_config(cards[i])
            for _,f in ipairs(card_config.weight) do
                value = value + f(self.database)
            end
            table.insert(card_list, {id = cards[i],value = value} )
        end
    end

    local max =-1
    for _,c in ipairs(card_list) do
        if c.value > max then
            max = c.value
            self.database.play_id = c.id
        end 
    end
    return true
end

function this:check_CardRandom(  )
    local card_list = {}
    local cards = GetPlayerManager().enemyCards
    self.database.play_id = -1
    if cards == nil or cards.Length == 0 then return true end
    local card_list = {}
    for i=0,cards.Length-1 do
        if cards[i] >= 0 then
            table.insert( card_list, cards[i] )
        end
    end
    if #card_list == 0 then return true end
    local index = math.random( 1,#card_list )
    self.database.play_id = card_list[index]
    return true
end



----------------------------------priority check (build database.target or database.target_pos)

function this.check_alive_friend( value,with_structure,num )
    return function ( database )
       return value
    end
end

----------------------------------target pos check
function this.check_pos_creature(  )
    return function ( database )
        local field = database.sess.field
        local close_unit = field:get_units(true,3-database.main_castle.side,database.main_castle,1)[1]
    if close_unit == nil then close_unit = database.sess.players[1].unit end
        local data = GetAssetManager():GetUnitData(database.play_id)
        local target_pos = {
            X = clamp(close_unit.transform.grid_pos.X,battle_def.ENEMYBOUND.column,battle_def.MAPMATRIX.column),
            Y = clamp(close_unit.transform.grid_pos.Y,0,battle_def.MAPMATRIX.row)
        }
        if data.attack_range > 16 then
            local shift = battle_def.MAPMATRIX.row/4
            local x = math.random(target_pos.X-shift,target_pos.X+shift)
            x= clamp(x,battle_def.ENEMYBOUND.column,battle_def.MAPMATRIX.column)
            local y = math.random( target_pos.Y-shift,target_pos.Y+shift )
            y= clamp(y,0,battle_def.MAPMATRIX.row)
            if math.abs( (x-close_unit.transform.grid_pos.X)<128 )then
                local tem =close_unit.transform.grid_pos.X +128
                if tem>=battle_def.ENEMYBOUND.column and tem<=battle_def.MAPMATRIX.column then
                    x = tem
                else
                    x = close_unit.transform.grid_pos.X -128
                end
            end
            if math.abs( (y-close_unit.transform.grid_pos.Y)<128 )then
                local tem =close_unit.transform.grid_pos.Y +128
                if tem>=0 and tem<=battle_def.MAPMATRIX.row then
                    y = tem
                else
                    y = close_unit.transform.grid_pos.Y -128
                end
            end
            target_pos.X = x
            target_pos.Y = y
        else
            local shift = battle_def.MAPMATRIX.row/16
            local x = math.random(target_pos.X-shift,target_pos.X+shift)
            --print("min"..target_pos.X-shift.."max"..target_pos.X+shift.."v"..x)
            x= clamp(x,battle_def.ENEMYBOUND.column,battle_def.MAPMATRIX.column)
            local y = math.random( target_pos.Y-shift,target_pos.Y+shift )
            --print("min"..target_pos.Y-shift.."max"..target_pos.Y+shift.."v"..y)
            y= clamp(y,0,battle_def.MAPMATRIX.row)
            -- if math.abs(x-close_unit.transform.grid_pos.X)<32 then
            --     local tem =close_unit.transform.grid_pos.X +32
            --     if tem>=battle_def.ENEMYBOUND.column and tem<=battle_def.MAPMATRIX.column then
            --         x = tem
            --     else
            --         x = close_unit.transform.grid_pos.X -32
            --     end
            -- end
            -- if math.abs(y-close_unit.transform.grid_pos.Y)<32 then
            --     local tem =close_unit.transform.grid_pos.Y +32
            --     if tem>=0 and tem<=battle_def.MAPMATRIX.row then
            --         y = tem
            --     else
            --         y = close_unit.transform.grid_pos.Y -32
            --     end
            -- end
            target_pos.X = x
            target_pos.Y = y
        end
        return target_pos
    end
end


return this