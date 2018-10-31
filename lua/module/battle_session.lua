
local this = class("battle_session")
local battle_field = require("module.battle.battle_field")
local battle_skill_mng = require("module.battle.battle_skill_manager")

function this:ctor(  )
    self.field = battle_field.new(self)
    self.skill_mng = battle_skill_mng.new(self)
    self.map = GetMapField()
end

function this:update( delta )
    self.field:update(delta)
    self.skill_mng:update(delta)
end

return this