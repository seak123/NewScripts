local this = class("ai_decorator")
local battle_def = require("module.battle.battle_def")
local config_mng = require("config.config_manager")

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
    local cards = {}
    local data = self.database.data
    for _,v in ipairs(data) do
        local value = 0
        local card_config = config_mng.get_card_config(v.id)
        for _,f in ipairs(card_config.decorators) do
            value = value + f(self.database)
        end
        table.insert(cards, {card = v,value = value} )
    end
    local max =0
    for _,c in ipairs(cards) do
        if c.value > max then
            max = c.value
            self.database.card = c.card
        end 
    end
    return true
end

function this:check_CardAvaliable(  )
    local cost = self.database.card.cost
    if self.database.data.saving >= cost then
        return true
    end
    return false
end
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
function this.check_pos_(  )
    -- body
end


return this