


local this = class("sample")
local session = require("module.battle_session")
local creature = require("module.battle.unit.creature")



function this.init( root )
    root.session = session.new()
end

return this