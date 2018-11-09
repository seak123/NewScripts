local base = require("module.battle.skill.raw_skill.base_rawskill")
local this = class("summon",base)

local factor = 8

function this:ctor(vo,database)
    self.vo = vo
    self.database = database
	self:init_build(vo)
end

function this:execute(sess,target)
    local field = sess.field
    local pos = self.database.target_pos
    local pos_array = self:get_pos_array(pos,self.vo.num)
    for i =1,self.vo.num do
        self.vo.data.init_x = pos_array[i].X
        self.vo.data.init_y = pos_array[i].Y
        field:add_unit(self.vo.data)
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

return this