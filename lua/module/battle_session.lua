
local this = class("battle_session")
local battle_field = require("module.battle.battle_field")
local battle_skill_mng = require("module.battle.battle_skill_manager")
local battle_trace = require("module.battle.battle_trace")
local battle_def = require("module.battle.battle_def")
local battle_trig = require("module.battle.battle_trigger")

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
end

function this:init_player_data( vo )
    for n,_ in ipairs(battle_def.PLAYERPROP) do
        self.players[1][n] = vo.player[n]
        self.players[2][n] = vo.enemy[n]
    end
end

return this