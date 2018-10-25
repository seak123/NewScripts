
local this = class("action_node")

function this:ctor( vo ,database)
      self.vo = vo
      self.action_type = vo.action_type
      self.decorators = {}
      self.running = false
      --init
      self:init_data(database)
      if self.vo.decorator ~= nil then
          for _,v in ipairs(self.vo.decorator) do
              local dec = require(v.execute).new(v)
              dec:init_data(database)
              table.insert( self.decorators, dec )
          end
      end
end

function this:init_data( database )
    self.database = database
end

function this:update(  )
    for _,v in ipairs(self.decorators) do
        if v:check() == false then
            return "failure"
        end
    end
    return self["update_"..self.action_type](self)
end

function this:update_Move(  )
    self.database.master.transform.des_pos = self.database.des_pos
    return "completed"
end

function this:update_MoveUnit(  )
    local field = self.database.master.sess.field
    
    if self.database.enemy ~= nil then
        if field:distance(self.database.enemy,self.database.master) <20 then
            return "completed"
        end
        self.database.master.transform.des_pos =  self.database.enemy.transform.grid_pos
        return "running"
    end
    return "failure"
end

function this:update_Attack(  )
    print("attacking !!!!!!!!!")
    return "running"
end

return this