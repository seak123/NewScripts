
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
    [20001] = "config.hero.1_natural_artemis.units.20001_porcupine",
    [20002] = "config.hero.1_natural_artemis.units.20002_sprite_deer"
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
    [10001] = {
        config = {"attack","skills","passives"},
        attack = {
            [0] = "config.hero.1_natural_artemis.attack.artemis_attack0",
            [1] = "config.hero.1_natural_artemis.attack.artemis_attack1",
            [2] = "config.hero.1_natural_artemis.attack.artemis_attack2",
            [3] = "config.hero.1_natural_artemis.attack.artemis_attack3",
            [4] = "config.hero.1_natural_artemis.attack.artemis_attack4"
        },
        skills = {
            [101] = "config.hero.1_natural_artemis.skills.artemis_skill101",
            [102] = "config.hero.1_natural_artemis.skills.artemis_skill102",
            [103] = "config.hero.1_natural_artemis.skills.artemis_skill103",
            [104] = "config.hero.1_natural_artemis.skills.artemis_skill104",
        },
        passives = {
            [101] = "config.hero.1_natural_artemis.passives.artemis_passive1",
            [102] = "config.hero.1_natural_artemis.passives.artemis_passive2",
            [103] = "config.hero.1_natural_artemis.passives.artemis_passive3",
            [104] = "config.hero.1_natural_artemis.passives.artemis_passive4"
        }

    }
}

function this.get_unit_config( id )
    local unit_vo = {
        ai_vo = this.unit_config[id].ai_vo,
        normal_attack = this.unit_config[id].normal_attack,
        skills = {this.unit_config[id].sp_attr[1]}
    }
    return this.unit_config[id]
end

function this.get_hero_config( data )
    local unit_config = {}
    local hero_conf = this.hero_config[data.id]
    unit_config.ai_vo = require("config.ai_config.normal_defence_ai")
    unit_config.skills = {}
    unit_config.passives = {}
    --unit_config.normal_attack = require(hero_conf.attack[0])


    local skill_flag = 1
    local passive_flag = 1

    for i=1,3 do
        local Lv = data["Skill"..i.."Lvl"]
        local attr = hero_conf.config[i]
        if attr == "attack" then
            unit_config.normal_attack = require(hero_conf.attack[Lv])
        elseif attr == "skills" then
            skill_flag = skill_flag + 1
            if Lv ~= 0 then
                table.insert( unit_config.skills, require(hero_conf.skills[skill_flag*100+Lv]) )
            end
        else
            passive_flag = passive_flag + 1
            if Lv ~=0 then
                table.insert( unit_config.passives, require(hero_conf.passives[passive_flag*100+Lv]) )
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