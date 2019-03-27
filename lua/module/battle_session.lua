
local this = class("battle_session")
local battle_field = require("module.battle.battle_field")
local battle_map = require("module.battle.battle_map")
local battle_skill_mng = require("module.battle.battle_skill_manager")
local battle_trace = require("module.battle.battle_trace")
local battle_def = require("module.battle.battle_def")
local battle_trig = require("module.battle.battle_trigger")
local battle_ai = require("module.battle.battle_ai.ai_tree")
local normal_ai = require("config.ai_config.normal_player_ai")

function this:ctor( vo )
    self.vo = vo
    self.map = GetMapField()
    self.battle_map = battle_map.new(self)
    self.field = battle_field.new(self)
    self.skill_mng = battle_skill_mng.new(self)
    self.trace = battle_trace.new(self)
    self.trigger = battle_trig.new(self)
    
    
    self.effect_mng = GetEffectManager()

    self.players = {{side = 1},{side = 2}}
    self:init_battle_data(vo)

    -- record every frame deltatime
    self.deltatime = 0

    -- record enemy process
    self.summoun_interval = 0.5
    self.summoun_process = 0
    self.summoun_count = 0
end

function this:update( delta )

    self.deltatime = delta
    self:update_enemys(delta)
    self.field:update(delta)
    self.skill_mng:update(delta)
    --self.ai:execute()
    self:check_result()
end

function this:init_battle_data( vo )
    for i=0,vo.units.Length-1 do
        self.field:add_unit( vo.units[i],-1)
    end
    self.enemy_num = vo.enemys.Length
    self.boss_vo = vo.boss
    self.field:add_boss(self.boss_vo)
end

function this:update_enemys( delta )
    if self.summoun_count >= self.enemy_num then return end
    self.summoun_process = self.summoun_process + delta
    if self.summoun_process > self.summoun_interval then
        self.field:add_unit(self.vo.enemys[self.summoun_count],-1)
        self.summoun_process = 0
        self.summoun_count = self.summoun_count + 1
        self.summoun_interval = math.random()*0.5 + 0.5
    end
end

function this:check_result(  )
    local res = self.field:check_result()
    if res ~= -1 then
        BattleCompleted(res)
    end
end

return this