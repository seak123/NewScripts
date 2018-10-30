local base = require("module.battle.skill.row_skill.base_rowskill")
local this = class("damage",base)

function this:ctor( vo )
    -- body
end

function this:update( delta ,database)
    return true
end

return this