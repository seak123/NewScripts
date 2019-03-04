
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
    self:init_battle_data(vo)

    -- record every frame deltatime
    self.deltatime = 0
end

function this:update( delta )

    self.deltatime = delta
    self.field:update(delta)
    self.skill_mng:update(delta)
    --self.ai:execute()
    self:check_result()
end

function this:init_battle_data( vo )
    for i=0,vo.unitNum-1 do
        self.field:add_unit( vo.units[i],-1)
    end
   
end

function this:check_result(  )

    
end

return this