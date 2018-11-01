
local this = {}

this.unit_config = {
    [1] = "config.unit.knight.knight_unit",
    [2] = "config.unit.knight.knight_unit"
}

function this.get_config_path( id )
    return this.unit_config[id]
end

return this