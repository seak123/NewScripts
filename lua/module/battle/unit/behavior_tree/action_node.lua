
local this = class("action_node")
local transform = require("module.battle.unit.component.transform")
local decorator = require("module.battle.unit.behavior_tree.data.decorator_vo")
local battle_def = require("module.battle.battle_def")

function this:ctor( vo ,database)
      self.vo = vo
      self.action_type = vo.action_type
      self.decorators = {}
      self.running = false
      self.max_runtime = 5
      --init
      self:init_data(database)
      if self.vo.decorators ~= nil then
          for _,v in ipairs(self.vo.decorators) do
              local dec = require(v.execute).new(v)
              dec:init_data(database)
              table.insert( self.decorators, dec )
          end
      end
end

function this:init_data( database )
    self.database = database
end

function this:init(  )
    self.runtime = 0
end

function this:update( delta )
    if self.running == false then
        for _,v in ipairs(self.decorators) do
            if v:check() == false then
                return "failure"
            end
        end
        if self["enter_"..self.action_type] ~= nil then
            if self["enter_"..self.action_type](self) == false then
                return "failure"
            end
        end
    end

    return self["update_"..self.action_type](self,delta)
end

function this:abort(  )
    if self["abort_"..self.action_type] ~= nil then
        self.running = false
        return self["abort_"..self.action_type](self)
    end
end

function this:enter_MoveForward(  )
    if self.database.master.statectrl:has_feature("de_move") then return false end
    -- print("uid:"..self.database.master.uid.." enter forward state")
    self.database.master.entity:AnimCasterAction(transform.AnimationState.Walk)
    self.runtime = 0
    self.max_runtime = 2
    return true
end

function this:abort_MoveForward(  )
    self.database.master.entity:AnimCasterBreak()
end

function this:update_MoveForward( delta )
    local field = self.database.master.sess.field
    local transform = self.database.master.transform
    local grid_pos = transform.grid_pos
    local next_room = transform.des_room
    local now_room = self.database.master.location
    local now_center = self.database.master.sess.battle_map:get_room_center(now_room)
    if next_room == now_room then
        return "completed"
    end
    local flag = nil
    if self.database.des_pos == nil then self.database.des_pos = {} end
    if math.modf(next_room/10) == math.modf(now_room/10) then
        local now_col = math.fmod( now_room,10 )
        local next_col = math.fmod( next_room,10 )
        if now_col>next_col then
            self.database.des_pos.X = now_center.X
            self.database.des_pos.Y = now_center.Y + battle_def.room_bound/2
            flag = 2
        else
            self.database.des_pos.X = now_center.X
            self.database.des_pos.Y = now_center.Y - battle_def.room_bound/2
            flag = 4
        end
    else
        local now_row = math.modf(now_room/10)
        local next_row = math.modf( next_room/10 )
        if now_row>next_row then
            self.database.des_pos.X = now_center.X + battle_def.room_bound/2
            self.database.des_pos.Y = now_center.Y
            flag = 3
        else
            self.database.des_pos.X = now_center.X - battle_def.room_bound/2
            self.database.des_pos.Y = now_center.Y
            flag = 1
        end
    end
    self.database.master.transform.des_pos = self.database.des_pos
    if field:distance(self.database.master.transform.grid_pos,self.database.des_pos) < 40 then
        field:portal(self.database.master,next_room,flag)
        self.running = false
        return "completed"
    end
    self.running = true
    self.runtime = self.runtime + delta
    -- if self.runtime > self.max_runtime then
    --     self.running = false
    --      return "failure" 
    -- end
    return "running"
end

function this:enter_MoveToPos(  )
    if self.database.master.statectrl:has_feature("de_move") then return false end
    -- print("uid:"..self.database.master.uid.." enter movetopos state")
    self.database.master.entity:AnimCasterAction(transform.AnimationState.Walk)
    self.runtime = 0
    self.max_runtime = 2
    return true
end

function this:abort_MoveToPos(  )
    self.database.master.entity:AnimCasterBreak()
end

function this:update_MoveToPos( delta )
    -- print("uid:"..self.database.master.uid.." update movetopos state")
    self.database.master.transform.des_pos = self.database.des_pos
    if self.database.master.transform.grid_pos.X == self.database.des_pos.X and self.database.master.transform.grid_pos.Y == self.database.des_pos.Y then
        self.running = false
        return "completed"
    end
    self.running = true
    self.runtime = self.runtime + delta
    -- if self.runtime > self.max_runtime then
    --     self.running = false
    --      return "failure" 
    -- end
    return "running"
end

function this:enter_MoveToEnemy(  )
    if self.database.master.statectrl:has_feature("de_move") then return false end
    -- print("uid:"..self.database.master.uid.." enter movetoenemy state")
    self.database.master.entity:AnimCasterAction(transform.AnimationState.Walk)
    self.runtime = 0
    self.max_runtime = 2
    self.enter_pos = {X = self.database.master.transform.grid_pos.X,
                      Y = self.database.master.transform.grid_pos.Y }
    return true
end

function this:abort_MoveToEnemy(  )
    self.database.master.entity:AnimCasterBreak()
end

function this:update_MoveToEnemy( delta )
    local field = self.database.master.sess.field
    if self.database.target ~= nil and self.database.target.alive== 0 then
        if field:distance(self.database.target,self.database.master) < 1.5*(self.database.master.data.radius + self.database.target.data.radius) then
            self.running = false
            return "completed"
        end
        self.database.master.transform.des_pos =  self.database.target.transform.grid_pos
        self.running = true
        self.runtime = self.runtime + delta
    
        -- if self.runtime > self.max_runtime then 
        --     self.running = false
        --     -- move a bit means cannot move to this enemy, decrease the threat_value
        --     if field:distance(self.enter_pos,self.database.master) < self.max_runtime*battle_def.MinSpeed/2 then
        --         self.database.master.threat_value[self.database.target.uid] = self.database.master.threat_value[self.database.target.uid] - 1
        --     end
        --     return "failure" 
        -- end
        if self.runtime > 2 then
            if field:distance(self.enter_pos,self.database.master) < battle_def.MinSpeed then
               self.database.master.threat_value[self.database.target.uid] = self.database.master.threat_value[self.database.target.uid] - 1
               self.running = false
               return "failure"
            end
            local de_find_enemy = decorator.new()
            de_find_enemy.type = decorator.Type.EnemyAround
            local dec = require(de_find_enemy.execute).new(de_find_enemy)
            dec:init_data(self.database)
            dec:check()
            self.runtime = 0
            self.enter_pos = {
                X = self.database.master.transform.grid_pos.X,
                Y = self.database.master.transform.grid_pos.Y
            }
        end
        
        return "running"
    end
    self.running = false
    return "failure"
end

function this:enter_Attack(  )
    if self.database.master.statectrl:has_feature("de_attack") then return false end
    -- print("uid:"..self.database.master.uid.." enter attack state")
    local base_interval = self.database.master.property:get("base_attack_interval")
    local attack_rate = self.database.master.property:get("attack_rate")
    local rate = base_interval/(1+battle_def.Attack_Rate_Factor*attack_rate)

    self.database.master.entity:AnimCasterAttack(1/rate)
    self.database.master.entity:SetRotation(self.database.target.transform.grid_pos.X,self.database.target.transform.grid_pos.Y)
    self.database.master.attack_process = 0
    self.runtime = 0
    self.max_runtime = 5
    return true
end

function this:abort_Attack(  )
    self.database.master.attack_process = 0
    self.database.master.entity:AnimCasterBreak()
end

function this:update_Attack( delta )
    -- print("uid:"..self.database.master.uid.." update attack state")
    local base_interval = self.database.master.property:get("base_attack_interval")
    local attack_rate = self.database.master.property:get("attack_rate")
    local rate = base_interval/(1+battle_def.Attack_Rate_Factor*attack_rate)
    self.database.master.entity:SetAttackSpeed(1/rate)
    local flag = self.database.master:do_attack(delta*1/rate,self.database.target)
    if flag == false then
        self.running = true
        self.runtime = self.runtime + delta
        if self.runtime > self.max_runtime then
            self.running = false
            return "failure" 
        end
        return "running"
    else
        self.running = false
        self.database.pre_attack_target = self.database.target
        return "completed"
    end
end

function this:enter_Caster()
    if self.database.master.statectrl:has_feature("de_skill") then return false end
    self.database.master.entity:AnimCasterAction(transform.AnimationState.Caster)
    self.runtime = 0
    self.max_runtime = 5
    return true
end

function this:abort_Caster(  )
    self.database.master.skill_process = 0
    self.database.master.entity:AnimCasterBreak()
end

function this:update_Caster(delta)
    local target = {}
    local target_pos = {X =0,Y =0}
    if self.database.target ~= nil then
        target = self.database.target
    end
    if self.database.target_pos ~= nil then
        target_pos = self.database.target_pos
    end
    local flag = self.database.master:do_skill(delta,target,target_pos,self.database.skill_index)
    if flag == false then
        self.running = true
        self.runtime = self.runtime + delta
        if self.runtime > self.max_runtime then
            self.running = false
            return "failure"
        end
        return "running"
    else
        self.running = false
        --self:abort_Caster()
        return "completed"
    end
end

function this:enter_Appear(  )
    if self.database.master.appeared == 1 then return false end
    self.database.master.entity:Appear()
    --self.database.master.entity:AnimCasterAction(transform.AnimationState.Appear)
    self.runtime = 0
    return true
end

function this:abort_Appear(  )
    self.database.master.entity:AnimCasterBreak()
end

function this:update_Appear(delta  )
    local flag = self.database.master:do_appear(delta)
    if flag == false then
        self.running = true
        return "running"
    else
        self.running = false
        --self:abort_Appear()
        return "completed"
    end
end

function this:enter_StayBack(  )
    --print("@@@@@enter stayback")
    if self.database.master.statectrl:has_feature("de_move") then return false end
    -- print("uid:"..self.database.master.uid.." enter forward state")
    self.database.master.entity:AnimCasterAction(transform.AnimationState.Walk)
    self.runtime = 0
    self.max_runtime = 2
    self.enter_pos = {X = self.database.master.transform.grid_pos.X,
                      Y = self.database.master.transform.grid_pos.Y }
    return true
end

function this:abort_StayBack(  )
    self.database.master.entity:AnimCasterBreak()
end

function this:update_StayBack( delta )
    --print("@@@@@update stayback")
    local field = self.database.master.sess.field
    self.database.des_pos = {X = self.database.master.data.init_x,Y = self.database.master.data.init_y}
    
    self.database.master.transform.des_pos = self.database.des_pos
    
    if field:distance(self.database.des_pos,self.database.master) < 2 then
        self.running = false
        return "completed"
    end
    
    if self.runtime > self.max_runtime then
        if field:distance(self.enter_pos,self.database.master) < battle_def.MinSpeed * 2 then
            self.running = false
            return "completed"
        end
    end
    self.running = true
    self.runtime = self.runtime + delta
    return "running"
end

function this:enter_Idle(  )
    self.database.master.entity:AnimCasterBreak()
end

function this:abort_Idle(  )
    self.database.master.entity:AnimCasterBreak()
    self.database.master.idle_time = 0
end

function this:update_Idle( delta )
    --print("@@stay idle")
    self.database.master.entity:AnimCasterBreak()
    self.database.master.idle_time = self.database.master.idle_time + delta
    self.running = true
    return "running"
end

return this