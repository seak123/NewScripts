
local this = class("action_node")
local transform = require("module.battle.unit.component.transform")
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
            self["enter_"..self.action_type](self)
        end
    end

    return self["update_"..self.action_type](self,delta)
end

function this:abort(  )
    if self["abort_"..self.action_type] ~= nil then
        return self["abort_"..self.action_type](self)
    end
end

function this:enter_MoveToPos(  )
    self.database.master.entity:AnimCasterAction(transform.AnimationState.Walk)
    self.runtime = 0
    self.max_runtime = 2
end

function this:update_MoveToPos( delta )
    self.database.master.transform.des_pos = self.database.des_pos
    if self.database.master.transform.grid_pos.X == self.database.des_pos.X and self.database.master.transform.grid_pos.Y == self.database.des_pos.Y then
        self.running = false
        return "completed"
    end
    self.running = true
    self.runtime = self.runtime + delta
    if self.runtime > self.max_runtime then
        self.running = false
         return "failure" 
    end
    return "running"
end

function this:enter_MoveToEnemy(  )
    self.database.master.entity:AnimCasterAction(transform.AnimationState.Walk)
    self.runtime = 0
    self.max_runtime = 1
    self.enter_pos = {X = self.database.master.transform.grid_pos.X,
                      Y = self.database.master.transform.grid_pos.Y }
end

function this:abort_MoveToEnemy(  )
    self.database.master.entity:AnimCasterBreak()
end

function this:update_MoveToEnemy( delta )
    local field = self.database.master.sess.field
    if self.database.enemy ~= nil then
        if field:distance(self.database.enemy,self.database.master) < 1.5*(self.database.master.data.radius + self.database.enemy.data.radius) then
            self.running = false
            return "completed"
        end
        self.database.master.transform.des_pos =  self.database.enemy.transform.grid_pos
        self.running = true
        self.runtime = self.runtime + delta
    
        if self.runtime > self.max_runtime then 
            self.running = false
            -- move a bit means cannot move to this enemy, decrease the threat_value
            if field:distance(self.enter_pos,self.database.master) < self.max_runtime*battle_def.MinSpeed/2 then
                self.database.master.threat_value[self.database.enemy.uid] = self.database.master.threat_value[self.database.enemy.uid] - 1
            end
            return "failure" 
        end
        
        return "running"
    end
    self.running = false
    return "failure"
end

function this:enter_Attack(  )
    self.database.master.entity:AnimCasterAttack(self.database.master.property:get("attack_rate"))
    self.database.master.entity:SetRotation(self.database.enemy.transform.grid_pos.X,self.database.enemy.transform.grid_pos.Y)
    self.database.master.attack_process = 0
    self.runtime = 0
    self.max_runtime = 5
end

function this:abort_Attack(  )
    self.database.master.attack_process = 0
    self.database.master.entity:AnimCasterBreak()
end

function this:update_Attack( delta )
    local flag = self.database.master:do_attack(delta*self.database.master.property:get("attack_rate"),self.database.enemy)
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
        return "completed"
    end
end

function this:enter_Caster()
    self.database.master.entity:AnimCasterAction(transform.AnimationState.Caster)
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
        return "completed"
    end
end

return this