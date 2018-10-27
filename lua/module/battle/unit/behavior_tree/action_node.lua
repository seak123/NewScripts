
local this = class("action_node")

function this:ctor( vo ,database)
      self.vo = vo
      self.action_type = vo.action_type
      self.decorators = {}
      self.running = false
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
    -- body
end

function this:update(  )
    if self.running == false then
        for _,v in ipairs(self.decorators) do
            if v:check() == false then
                return "failure"
            end
        end
    end
    return self["update_"..self.action_type](self)
end

function this:abort(  )
    if self["abort_"..self.action_type] ~= nil then
        return self["abort_"..self.action_type](self)
    end
end

function this:update_MoveToPos(  )
    self.database.master.transform.des_pos = self.database.des_pos
    if self.database.master.transform.grid_pos.X == self.database.des_pos.X and self.database.master.transform.grid_pos.Y == self.database.des_pos.Y then
        return "completed"
    end
    return "running"
end

function this:update_MoveToUnit(  )
    local field = self.database.master.sess.field
    if self.database.enemy ~= nil then
        if field:distance(self.database.enemy,self.database.master) < 1.5*(self.database.master.data.radius + self.database.enemy.data.radius) then
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