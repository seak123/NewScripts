
local this = class("battle_session")
local field = require("module.strategy.strategy_field")
local battle_map = require("module.battle.battle_map")
local battle_trace = require("module.battle.battle_trace")
local battle_def = require("module.battle.battle_def")
local battle_trig = require("module.battle.battle_trigger")
local battle_ai = require("module.battle.battle_ai.ai_tree")
local normal_ai = require("config.ai_config.normal_player_ai")

function this:ctor( vo )
    self.vo = vo
    self.map = GetMapField()
    self.battle_map = battle_map.new(self)
    self.field = field.new(self)
    self.trace = battle_trace.new(self)
    self.trigger = battle_trig.new(self)

    self.effect_mng = GetEffectManager()

    self:init_battle_data(vo)

end

function this:update( delta )

    self.field:update(delta)

end

function this:init_battle_data( vo )
    for i=0,vo.units.Length-1 do
        self.field:add_unit( vo.units[i])
    end
    self.boss_vo = vo.boss
    self.field:add_unit(self.boss_vo)
end


return this