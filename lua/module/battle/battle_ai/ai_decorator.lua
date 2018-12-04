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
    local cards = self.database.cards
    self.database.play_id = -1
    for i=0,cards.Length-1 do
        local value = 0
        local card_config = config_mng.get_card_config(cards[i])
        for _,f in ipairs(card_config.weight) do
            value = value + f(self.database)
        end
        table.insert(card_list, {id = cards[i],value = value} )
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

-- function this:check_CardAvaliable(  )
--     local cost = self.database.card.cost
--     if self.database.data.saving >= cost then
--         return true
--     end
--     return false
-- end
-- function this:check_Forward(  )
--     local transform = self.database.master.transform
--     local grid_pos = transform.grid_pos
--     if self.database.master.side == 1 then
--         self.database.des_pos = {X = battle_def.MAPMATRIX.column,Y = grid_pos.Y}
--     else
--         self.database.des_pos = {X = 0,Y = grid_pos.Y}
--     end
--     return true
-- end



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
        local close_unit = field:find_enemy(true,database.main_castle)
        local data = database.card
        local target_pos = {
            X = clamp(close_unit.transform.grid_pos.X,0,battle_def.MAPMATRIX.column),
            Y = clamp(close_unit.transform.grid_pos.Y,0,battle_def.MAPMATRIX.row)
        }
        if data.attack_range > 16 then
            local shift = battle_def.MAPMATRIX.row/4
            local x = math.random(target_pos.X-shift,target_pos.X+shift)
            x= clamp(x,0,battle_def.MAPMATRIX.column)
            local y = math.random( target_pos.Y-shift,target_pos.Y+shift )
            y= clamp(y,0,battle_def.MAPMATRIX.row)
            if math.abs( (x-close_unit.transform.grid_pos.X)<128 )then
                local tem =close_unit.transform.grid_pos.X +128
                if tem>=0 and tem<=battle_def.MAPMATRIX.column then
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
                    x = close_unit.transform.grid_pos.Y -128
                end
            end
            target_pos.X = x
            target_pos.Y = y
        else
            local shift = battle_def.MAPMATRIX.row/16
            local x = math.random(target_pos.X-shift,target_pos.X+shift)
            x= clamp(x,0,battle_def.MAPMATRIX.column)
            local y = math.random( target_pos.Y-shift,target_pos.Y+shift )
            y= clamp(y,0,battle_def.MAPMATRIX.row)
            if math.abs( (x-close_unit.transform.grid_pos.X)<32 )then
                local tem =close_unit.transform.grid_pos.X +32
                if tem>=0 and tem<=battle_def.MAPMATRIX.column then
                    x = tem
                else
                    x = close_unit.transform.grid_pos.X -32
                end
            end
            if math.abs( (y-close_unit.transform.grid_pos.Y)<32 )then
                local tem =close_unit.transform.grid_pos.Y +32
                if tem>=0 and tem<=battle_def.MAPMATRIX.row then
                    y = tem
                else
                    x = close_unit.transform.grid_pos.Y -32
                end
            end
            target_pos.X = x
            target_pos.Y = y
        end
        return target_pos
    end
end


return this