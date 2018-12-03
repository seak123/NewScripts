
local this = class("ai_action")
local battle_def = require("module.battle.battle_def")
local config_mng = require("config.config_manager")

function this:ctor( vo ,database)
      self.vo = vo
      self.action_type = vo.action_type
      self.decorators = {}

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

function this:execute( )
    for _,v in ipairs(self.decorators) do
        if v:check() == false then
            return false
        end
    end

    return self["enter_"..self.action_type](self)
end

function this:enter_Wait(  )
    return true
end

function this:enter_Caster(  )
    local card_config = config_mng.get_card_config(self.database.play_id)
    local target_pos = card_config.target(self.database)
    GetPlayerManager().EnemyPlayCard(self.database.play_id)
    return true
end

-- function this:enter_MoveForward(  )
--     if self.database.master.statectrl:has_feature("de_move") then return false end
--     self.database.master.entity:AnimCasterAction(transform.AnimationState.Walk)
--     self.runtime = 0
--     self.max_runtime = 2
--     return true
-- end

-- function this:abort_MoveForward(  )
--     self.database.master.entity:AnimCasterBreak()
-- end

-- function this:update_MoveForward( delta )
--     local transform = self.database.master.transform
--     local grid_pos = transform.grid_pos
--     if self.database.master.side == 1 then
--         self.database.des_pos = {X = battle_def.MAPMATRIX.column,Y = grid_pos.Y}
--     else
--         self.database.des_pos = {X = 0,Y = grid_pos.Y}
--     end
--     self.database.master.transform.des_pos = self.database.des_pos
--     if self.database.master.transform.grid_pos.X == self.database.des_pos.X and self.database.master.transform.grid_pos.Y == self.database.des_pos.Y then
--         self.running = false
--         return "completed"
--     end
--     self.running = true
--     self.runtime = self.runtime + delta
--     if self.runtime > self.max_runtime then
--         self.running = false
--          return "failure" 
--     end
--     return "running"
-- end



return this