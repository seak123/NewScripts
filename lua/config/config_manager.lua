
local this = {}
local decorator = require("module.battle.battle_ai.ai_decorator")

this.unit_config = {
    [0] = "config.unit.0_castle.castle_unit",
    [1] = "config.unit.1_knight.knight_unit",
    [2] = "config.unit.2_slowknight.slowknight_unit",
    [3] = "config.unit.3_ent.ent_unit",
    [4] = "config.unit.4_fire_apprentice.fire_apprentice_unit",
    [5] = "config.unit.5_ent_forest.ent_forest_unit",
    [1011] = "config.unit.101_natural_main_castle.natural_main_castle_unit",
    [1071] = "config.unit.107_natural_statue.natural_statue_unit",
    [1081] = "config.unit.108_elf_archer.elf_archer_unit",
    [1091] = "config.unit.109_elf_pikeman.elf_pikeman_unit",
    [5011] = "config.unit.501_ice_dragon.ice_dragon_unit",
    [6011] = "config.unit.601_fire_tower.fire_tower_unit",
    [20001] = "config.hero.1_natural_artemis.units.20001_porcupine",
    [20002] = "config.hero.1_natural_artemis.units.20002_sprite_deer"
}

this.skill_config = {
    [1] = "config.skill_config.common_skill.1_summon"
}

this.common_attr = {
    
}

this.card_config = {
    [1] = {target=decorator.check_pos_creature()},
    [1081] = {target=decorator.check_pos_creature()},
    [1091] = {target=decorator.check_pos_creature()},
    [5011] = {target=decorator.check_pos_creature()}
    
}

this.hero_config = {
    [10002] = "config.hero.2_lava_boss.unit.lava_unit",
}

function this.get_unit_config( data )
    local id = data.id
    local config = require(this.unit_config[id])
    local unit_vo = {
        normal_attack = config.normal_attack,
        skills = {}
    }
    if data.side == 1 then
        if data.type == 0 then
            unit_vo.ai_vo =  require("config.ai_config.normal_defence_ai")
        else
            unit_vo.ai_vo = require("config.ai_config.normal_structure_ai")
        end
    else
        unit_vo.ai_vo = require("config.ai_config.normal_ai")
    end
    for _,v in ipairs(config.sp_attr) do
        table.insert( unit_vo.skills, v)
    end
    for _,v in ipairs(config.skills) do
        table.insert( unit_vo.skills, v)
    end
    return unit_vo
end

function this.get_hero_config( data )
    local config = require(this.hero_config[data.id])
    local unit_vo = {
        normal_attack = config.normal_attack,
        ai_vo = require("config.ai_config.normal_boss_ai"),
        skills = {}
    }
    for _,v in ipairs(config.sp_attr) do
        table.insert( unit_vo.skills, v)
    end
    for _,v in ipairs(config.skills) do
        table.insert( unit_vo.skills, v)
    end
    return unit_vo
end

function this.get_skill_config( id )
    return this.skill_config[id]
end

function this.get_card_config( id )
    return this.card_config[id]
end

return this