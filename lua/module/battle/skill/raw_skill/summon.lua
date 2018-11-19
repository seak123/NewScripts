local base = require("module.battle.skill.raw_skill.base_rawskill")
local this = class("summon",base)
local def = require("module.battle.battle_def")

local factor = 8

function this:ctor(vo,database)
    self.vo = vo
    self.database = database
	self:init_build(vo)
end

function this:execute(sess,target)
    -- init
    local data = self.vo.data(self.database)
    if type(data) ~= "userdata" then
        data = self:get_unit_data(data)
    end
    if data.type == 0 then
        -- summon creature
        local num = self.vo.num(self.database)
        data.side = self.database.caster.unit.side

        local field = sess.field
        local pos = self.database.target_pos
        local pos_array = self:get_pos_array(pos,num)
        for i =1,num do

            data.init_x = this.clamp(pos_array[i].X,0,def.MAPMATRIX.column)
            data.init_y = this.clamp(pos_array[i].Y,0,def.MAPMATRIX.row)
            field:add_unit(data)
        end
    end
    if data.type == 1 then
        -- summon structure
        local field = sess.field
        local struct_uid = self.vo.num(self.database)
        data.side = self.database.caster.unit.side
        local pos = self.database.target_pos
        data.init_x = this.clamp(pos.X,0,def.MAPMATRIX.column)
        data.init_y = this.clamp(pos.Y,0,def.MAPMATRIX.row)
        field:add_unit(data,struct_uid)
    end

end

function this:get_pos_array( pos,num )
    local pos_array = {}
    local index = math.ceil(math.sqrt( num ))
    for i=1,num do
       local row = math.floor(i/index)
       local col = i - row*index
       table.insert( pos_array, {X=pos.X+(row-index)*factor,Y = pos.Y +(col-index)*factor })
    end
    return pos_array
end

function this:get_unit_data( id )
    -- body
end

function this.clamp( num,min,max )
    if num <min then
        num = min
    elseif num > max then
        num = max
    end
    return num
end


return this