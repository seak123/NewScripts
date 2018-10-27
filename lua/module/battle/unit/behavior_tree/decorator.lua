local this = class("decorator")
local battle_def = require("module.battle.battle_def")

function this:ctor( vo )
    self.type = vo.type
end

function this:init_data( database )
    self.database = database
end


function this:check(  )
    return self["check_"..self.type](self)
end

function this:check_Forward(  )
    local transform = self.database.master.transform
    local grid_pos = transform.grid_pos
    if self.database.master.side == 1 then
        self.database.des_pos = {X = battle_def.MAPMATRIX.column,Y = grid_pos.Y}
    else
        self.database.des_pos = {X = 0,Y = grid_pos.Y}
    end
    return true
end

function this:check_EnemyAround(  )
    local field = self.database.master.sess.field
    local unit = field:find_enemy(self.database.master)
    if unit == nil then
        return false
    end
    self.database.enemy = unit
    return true
end

function this:check_EnemyInAttackRange(  )
    local enemy = self.database.enemy
    local field = self.database.master.sess.field
    if enemy ~= nil then
        local dis = field:distance(enemy,self.database.master)
        if dis < (1.5*(self.database.master.data.radius + enemy.data.radius)+self.database.master.data.attack_range )then
            return true
        end
    end
    return false
end


return this