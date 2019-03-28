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
    if now_room == -1 then
        local next_room = self.database.master.sess.battle_map:get_entry_room()
        transform.des_room = next_room
        return true
    end
    if transform.des_room == now_room then
        if now_room == self.database.master.sess.battle_map:get_boss_room() then
            return false
        end
        local next_room = self:get_next_room_id(now_room)
        transform.des_room = next_room
    end
    return true
end

function this:get_next_room_id( now_id )
    local valid_rooms = {now_id-1,now_id+1,now_id+10,now_id-10}
    local new_rooms = {}
    
    for i=#valid_rooms,1,-1 do
        if self.database.master.sess.battle_map:get_room_center(valid_rooms[i]) == nil or valid_rooms[i] == self.database.master.last_location then
            table.remove( valid_rooms, i)
        end
    end

    for _, i in ipairs(valid_rooms) do
        local flag = true
        for _,j in ipairs(self.database.master.arrived_rooms) do
            if j == i then
                flag = false
            end      
        end
        if flag then table.insert( new_rooms, i ) end
    end

    if #new_rooms ~= 0 then
        for _,v in ipairs(new_rooms) do
            if v == self.database.master.sess.battle_map:get_boss_room() then
                return v
            end
        end
        local index = math.random(#new_rooms)
        return new_rooms[index]
    elseif #valid_rooms ~= 0 then
        local boss_room = self.database.master.sess.battle_map:get_boss_room()
        local boss_row = math.modf(boss_room/10)
        local boss_col = math.fmod(boss_room,10)
        local min = 9999
        local res
        for i,_ in ipairs(valid_rooms) do
            local row = math.modf(valid_rooms[i]/10 )
            local col = math.fmod( valid_rooms[i],10 )
            local dis = (row-boss_row)*(row-boss_row)+(col-boss_col)*(col-boss_col)
            
            if dis < min then
                min = dis
                res = i
            end
        end
        
        return valid_rooms[res]
    else
        return self.database.master.last_location
    end
end

function this:check_EnemyAround(  )
    local field = self.database.master.sess.field
    local unit
    if self.database.pre_attack_target ~= nil and self.database.pre_attack_target.alive ==0 and self.database.pre_attack_target.location == self.database.master.location then
        unit = self.database.pre_attack_target
    else
        local active = nil
        if self.database.master.side == 1 then
            active = 1
        else
            active = 0
        end
        unit = field:find_enemy(false,self.database.master,false,active)
    end
    if self.database.master.statectrl:has_feature("confused") then
        unit = field:find_enemy(false,self.database.master,true)
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
        if master.skills_coold[index].value == 0 and master.energy >= master.skills[index].energy then
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

function this:check_Boring(  )
    local master = self.database.master
    local delta = master.delta
    local bound
    if self.database.master.location == self.database.master.sess.battle_map:get_boss_room() then
        bound = battle_def.room_bound/3
    else
        bound = battle_def.room_bound/4
    end
    if math.modf(master.idle_time) < math.modf(master.idle_time +delta ) and math.random() < 0.2 then
        local pos = master.transform.grid_pos
        local center = master.sess.battle_map:get_room_center(master.location)
        local x = clamp(pos.X + math.random(-100,100),center.X - bound,center.X + bound)
        local y = clamp(pos.Y + math.random(-100,100),center.Y - bound,center.Y + bound)
        self.database.des_pos = {X = x,Y= y}
        
        return true
    end
    return false
end

function this:check_NeedBack(  )
    local master = self.database.master
    local center = master.sess.battle_map:get_room_center(master.location)
    local dis = self.database.master.sess.field:distance(master,center)
    if dis > 8 then
        self.database.des_pos = {X = center.X,Y = center.Y}
        return true
    else
        return false
    end
end


----------------------------------skill check (build database.target or database.target_pos)

function this.check_skill_EnemyInRange(with_structure,range)
    return function (database)
    local field = database.master.sess.field
    local enemy = field:find_enemy(with_structure,database.master,false,1,range)
    if enemy ~= nil then
        database.target = enemy
        return true
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
        local unit = field:find_random_unit(with_structure,database.master.side,database.master,func)
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