local this = class("ai_decorator")
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



----------------------------------skill check (build database.target or database.target_pos)

-- function this.check_skill_EnemyInRange(range,with_structure)
--     return function (database)
--     local field = database.master.sess.field
--     local enemy = field:find_enemy(with_structure,database.master)
--     if enemy ~= nil then
--         local dis = field:distance(enemy,database.master)
--         if dis < range then
--             database.target = enemy
--             return true
--         end
--     end
--     return false
-- end
-- end
----------------------------------priority check


return this