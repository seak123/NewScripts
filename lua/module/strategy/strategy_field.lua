
local this = class("battle_field")
local unit = require("module.strategy.strategy_unit")
local battle_def = require("module.battle.battle_def")
local bit = require("utils.bit_calc")

function this:ctor(sess )
    self.sess = sess
    self.units = {}
end

function this:add_unit( data)
    local unit = unit.new(self.sess,data)
    table.insert(self.units,unit)
end

function this:remove_entity( _uid )
    for k,u in ipairs(self.units) do
        if u.uid == _uid then
            u:remove()
            table.remove( self.units, k)
        end
    end
end

-- args can be unit or pos
function this:distance(a_unit,b_unit  )
    local a_pos = a_unit
    local b_pos = b_unit
    if a_unit.transform ~= nil then
        a_pos = {X = a_unit.transform.grid_pos.X,Y = a_unit.transform.grid_pos.Y}
    end
    if b_unit.transform ~= nil then
        b_pos = {X = b_unit.transform.grid_pos.X,Y = b_unit.transform.grid_pos.Y}
    end
    local x =a_pos.X - b_pos.X
    local y = a_pos.Y - b_pos.Y
    return math.sqrt( x*x + y*y )
end

function this:update( delta )
    
    for _,u in ipairs(self.units) do
        u:update(delta)
    end
end

return this