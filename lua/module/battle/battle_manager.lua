local this = {}
local session = require("module.battle_session")
local strategy_session = require("module.strategy.strategy_session")

this.session = nil

function this.battle_begin(vo)
    this.session = session.new(vo)
end

function this.battle_completed( res )
    this.session = nil
    BattleCompleted(res)
end

function this.strategy_begin( vo )
    this.session = strategy_session.new(vo)
end

return this