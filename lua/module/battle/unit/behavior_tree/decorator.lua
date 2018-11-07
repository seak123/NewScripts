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
    local unit = field:find_enemy(true,self.database.master)
    if self.database.master.statectrl:has_feature("confused") then
        unit = field:find_friend(true,self.database.master)
    end
    if unit == nil then
        return false
    end
    self.database.enemy = unit
    if self.database.master.statectrl:has_feature("taunt") then
        self.database.enemy = self.database.master.statectrl.taunt_target
    end
    return true
end

function this:check_EnemyInAttackRange(  )

    if self.database.master.statectrl:has_feature("confused") and self.database.enemy.side ~= self.database.master.side then
        return false
    end

    if self.database.master.statectrl:has_feature("taunt") then
        self.database.enemy = self.database.master.statectrl.taunt_target
    end

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

function this:check_SkillAvaliable()
    local master = self.database.master
    for index=#master.skills_coold,1,-1 do
        -- check skill coold is 0
        if master.skills_coold[index].value == 0 then
            local flag = true
            for _,v in ipairs(master.skills[index].decorators) do
                flag = flag and v(self.database)
            end
            if flag == true then
                self.database.skill_index = index
                return true
            end
        end
    end
    return false
end


----------------------------------skill check (build database.target or database.target_pos)

function this.check_skill_EnemyInRange(range)
    return function (database)
    local field = database.master.sess.field
    local enemy = field:find_enemy(true,database.master)
    if enemy ~= nil then
        local dis = field:distance(enemy,database.master)
        if dis < range then
            database.target = enemy
            return true
        end
    end
    return false
end
end


return this