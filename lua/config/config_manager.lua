
local this = {}
local decorator = require("module.battle.battle_ai.ai_decorator")

this.unit_config = {
    [0] = "config.unit.0_castle.castle_unit",
    [1] = "config.unit.1_knight.knight_unit",
    [2] = "config.unit.2_slowknight.slowknight_unit",
    [3] = "config.unit.3_ent.ent_unit",
    [4] = "config.unit.4_fire_apprentice.fire_apprentice_unit",
    [5] = "config.unit.5_ent_forest.ent_forest_unit",
    [101] = "config.unit.101_natural_main_castle.natural_main_castle_unit",
    [107] = "config.unit.107_natural_statue.natural_statue_unit",
    [108] = "config.unit.108_elf_archer.elf_archer_unit",
    [109] = "config.unit.109_elf_pikeman.elf_pikeman_unit",
    [501] = "config.unit.501_ice_dragon.ice_dragon_unit"
}

this.skill_config = {
    [1] = "config.skill_config.common_skill.1_summon"
}

this.card_config = {
    [1] = {target=decorator.check_pos_creature()},
    [1081] = {target=decorator.check_pos_creature()},
    [1091] = {target=decorator.check_pos_creature()},
    [5011] = {target=decorator.check_pos_creature()}
    
}

this.hero_config = {
    [1] = {
        attack = {
            [1] = "",
            [2] = ""
        },
        skills = {
            [101] = "",
            [102] = "",
            [103] = "",
            [104] = "",
            [201] = "",
            [202] = "",
            [203] = "",
            [204] = ""
        },
        passives = {

        }

    }
}

function this.get_unit_config( id )
    return this.unit_config[id]
end

function this.get_hero_config( data )
    local unit_config = {}
    local hero_conf = this.hero_config[data.id]
    unit_config.ai_vo = require("config.ai_config.normal_ai")

    unit_config.normal_attack = hero_conf.attack[data.attack_id]

    unit_config.skills = {}
    local skill_array = data.skills
    if skill_array ~= nil and skill_array.Length > 0 then
        for i=0,skill_array.Length-1 do
            if skill_array[i] >=0 then
                table.insert( unit_config, require(hero_conf.skills[skill_array[i]]) )
            end
        end
    end

    unit_config.passives = {}
    local pass_array = data.passives
    if pass_array ~= nil and pass_array.Length > 0 then
        for i=0,pass_array.Length-1 do
            if pass_array[i] >=0 then
                table.insert( unit_config, require(hero_conf.skills[pass_array[i]]) )
            end
        end
    end
    unit_config.battlecry = {}
    
    unit_config.deathrattle = {}
    return unit_config
end

function this.get_skill_config( id )
    return this.skill_config[id]
end

function this.get_card_config( id )
    return this.card_config[id]
end

return this