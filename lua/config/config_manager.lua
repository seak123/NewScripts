
local this = {}

this.unit_config = {
    [0] = "config.unit.0_castle.castle_unit",
    [1] = "config.unit.1_knight.knight_unit",
    [2] = "config.unit.2_slowknight.slowknight_unit",
    [3] = "config.unit.3_ent.ent_unit",
    [4] = "config.unit.4_fire_apprentice.fire_apprentice_unit",
    [5] = "config.unit.5_ent_forest.ent_forest_unit"
}

this.skill_config = {
    [1] = "config.skill_config.common_skill.1_summon"
}

function this.get_unit_config( id )
    return this.unit_config[id]
end

function this.get_skill_config( id )
    return this.skill_config[id]
end

return this