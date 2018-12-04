
local this = class("battle_session")
local battle_field = require("module.battle.battle_field")
local battle_skill_mng = require("module.battle.battle_skill_manager")
local battle_trace = require("module.battle.battle_trace")
local battle_def = require("module.battle.battle_def")
local battle_trig = require("module.battle.battle_trigger")
local battle_ai = require("module.battle.battle_ai.ai_tree")
local normal_ai = require("config.ai_config.normal_player_ai")

function this:ctor( vo )
    self.field = battle_field.new(self)
    self.skill_mng = battle_skill_mng.new(self)
    self.trace = battle_trace.new(self)
    self.trigger = battle_trig.new(self)
    self.map = GetMapField()
    self.effect_mng = GetEffectManager()

    self.players = {{side = 1},{side = 2}}
    self:init_player_data(vo)

    -- record every frame deltatime
    self.deltatime = 0
end

function this:update( delta )
    self.deltatime = delta
    self.field:update(delta)
    self.skill_mng:update(delta)
    self.ai:execute()
    self:check_result()
end

function this:init_player_data( vo )
    local player_data = vo.player.mainCastle
    player_data.side = 1
    player_data.init_x = 32
    player_data.init_y = battle_def.MAPMATRIX.row/2
    self.players[1].unit = self.field:add_unit(player_data,-1)

    local enemy_data = vo.enemy.mainCastle
    enemy_data.side = 2
    enemy_data.init_x = battle_def.MAPMATRIX.column - 32
    enemy_data.init_y = battle_def.MAPMATRIX.row/2
    self.players[2].unit = self.field:add_unit(enemy_data,-1)
    self.players[2].cards = vo.enemy.card_box
    self.ai = battle_ai:build(self,normal_ai)
end

function this:check_result(  )
    if self.players[1].unit.alive == 2 then
        print("players 1 lose")
        root.mng.battle_completed()
    elseif self.players[2].unit.alive == 2 then
        print("players 2 lose")
        root.mng.battle_completed()
    end
    
end

return this