local this = {}
local session = require("module.battle_session")

this.session = nil

function this.battle_begin(vo)
    this.session = session.new(vo)
end

function this.battle_completed( res )
    this.session = nil
    BattleCompleted(res)
end

return this