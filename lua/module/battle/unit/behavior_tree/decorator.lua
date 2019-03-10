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
    local now_room = self.database.master.location
    if transform.des_room == now_room then
        local next_room = this.get_next_room_id(now_room,math.random(4))
        while self.database.master.sess.battle_map:get_room_center(next_room) == nil do
            next_room = this.get_next_room_id(now_room,math.random(4))
        end
        transform.des_room = next_room
    end
    return true
end

function this.get_next_room_id( now_id,flag )
    if flag == 1 then
        return now_id + 10
    elseif flag == 2 then
        return now_id - 10
    elseif flag == 3 then
        return now_id + 1
    else
        return now_id - 1
    end
end

function this:check_EnemyAround(  )
    local field = self.database.master.sess.field
    local unit
    if self.database.pre_attack_target ~= nil and self.database.pre_attack_target.alive ==0 and self.database.pre_attack_target.type == 0 then
        unit = self.database.pre_attack_target
    else
        unit = field:find_enemy(true,self.database.master)
    end
    if self.database.master.statectrl:has_feature("confused") then
        unit = field:find_enemy(true,self.database.master,true)
    end
    if self.database.master.statectrl:has_feature("taunt") then
        unit = self.database.master.statectrl.taunt_target
    end
    if unit == nil or unit.alive ~= 0 then
        return false
    end
    self.database.target = unit
    
    return true
end

function this:check_EnemyInAttackRange(  )

    if self.database.master.statectrl:has_feature("confused") and self.database.target.side ~= self.database.master.side then
        return false
    end

    if self.database.master.statectrl:has_feature("taunt") then
        self.database.target = self.database.master.statectrl.taunt_target
    end

    local enemy = self.database.target
    
    local field = self.database.master.sess.field
    if enemy ~= nil and enemy.alive == 0 then
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

function this.check_skill_EnemyInRange(range,with_structure)
    return function (database)
    local field = database.master.sess.field
    local enemy = field:find_enemy(with_structure,database.master)
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

function this.check_hurt_friend_in_range( range,with_structure )
    return function ( database )
        local field = database.master.sess.field
        local func = function(unit)
            return unit.hp<unit.max_hp and field:distance(unit,database.master) <= range
        end
        local unit = field:find_random_unit(with_structure,database.master.side,func)
        if unit == nil then
            return false
        else
            database.target = unit
            return true
        end
    end
end

function this.check_summon(  )
    return function ( database )
    --    if database.master.sess.players[database.master.side]:has_feature("permit_summon") then
    --     return false
    --    end
        database.target_pos = database.master.transform.grid_pos
        return true
    end
end

function this.find_alive_friend(with_structure )
    return function ( database )
        local field = database.master.sess.field
        local friend = field:find_friend(with_structure,database.master)
        if friend ~= nil then
            database.target = friend
            return true
        end
        return false
    end
end


return this