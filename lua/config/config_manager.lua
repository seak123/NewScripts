
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
    [110] = "config.unit.110_large_angel.large_angel_unit",
    [301] = "config.unit.301_ice_dragon.ice_dragon_unit",
    [302] = "config.unit.302_wraith_magic_sword.wraith_magic_unit",
    [601] = "config.unit.601_fire_tower.fire_tower_unit",
    [602] = "config.unit.602_lighting_storm.lighting_storm_unit",
    [701] = "config.unit.701_natural_power.natural_power_unit",
}

this.skill_config = {
    [1] = "config.skill_config.common_skill.1_summon"
}

this.common_attr = {
    [108] = require("config.unit.108_elf_archer.elf_archer_sp_attr_1"),
    [109] = require("config.unit.109_elf_pikeman.elf_pikeman_sp_attr_1"),
    [302] = require("config.unit.302_wraith_magic_sword.wraith_magic_sp_attr"),
    [601] = require("config.unit.601_fire_tower.fire_tower_skill"),
    [602] = require("config.unit.602_lighting_storm.lighting_storm_skill")
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
            if data.con_type == 0 then
                unit_vo.ai_vo = require("config.ai_config.static_structure_ai")
            elseif data.con_type == 2 then
                unit_vo.ai_vo = require("config.ai_config.normal_structure_ai")
            end
        end
    else
        unit_vo.ai_vo = require("config.ai_config.normal_ai")
    end

    for i=0,data.skills.Length-1 do
        table.insert( unit_vo.skills, this.get_common_attr(data.skills[i]))
    end
    -- for _,v in ipairs(data.skills) do
        
    --     table.insert( unit_vo.skills, v)
    -- end
    return unit_vo
end

function this.get_hero_config( data )
    local config = require(this.hero_config[data.id])
    local unit_vo = {
        normal_attack = config.normal_attack,
        ai_vo = require("config.ai_config.normal_boss_ai"),
        skills = {}
    }
    for _,v in ipairs(config.skills) do
        local vo = this.get_common_attr(v)
        table.insert( unit_vo.skills, vo)
    end
    return unit_vo
end

function this.get_skill_config( id )
    return this.skill_config[id]
end

function this.get_card_config( id )
    return this.card_config[id]
end

function this.get_common_attr( id )
    return this.common_attr[id]
end

return this