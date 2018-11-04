
local this = {}

this.unit_config = {
    [1] = "config.unit.1_knight.knight_unit",
    [2] = "config.unit.2_slowknight.slowknight_unit",
    [3] = "config.unit.3_ent.ent_unit",
    [4] = "config.unit.4_fire_apprentice.fire_apprentice_unit"
}

function this.get_config_path( id )
    return this.unit_config[id]
end

return this