
local this = {}
local decorator = require("module.battle.battle_ai.ai_decorator")

this.unit_config = {
    [0] = "config.unit.0_castle.castle_unit",
    [1] = "config.unit.1_knight.knight_unit",
    [2] = "config.unit.2_slowknight.slowknight_unit",
    [3] = "config.unit.3_ent.ent_unit",
    [4] = "config.unit.4_fire_apprentice.fire_apprentice_unit",
    [5] = "config.unit.5_ent_forest.ent_forest_unit",
    [7] = "config.unit.7_natural_statue.natural_statue_unit"
}

this.skill_config = {
    [1] = "config.skill_config.common_skill.1_summon"
}

this.card_config = {
    [1] = {weight = {decorator.check_alive_friend(1,false,2)},target=decorator.check_pos_creature()}
}

function this.get_unit_config( id )
    return this.unit_config[id]
end

function this.get_skill_config( id )
    return this.skill_config[id]
end

function this.get_card_config( id )
    return this.card_config[id]
end

return this