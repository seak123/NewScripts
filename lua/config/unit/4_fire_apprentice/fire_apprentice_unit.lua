local this = {}

this.ai_vo = require("config.ai_config.normal_defence_ai")
this.normal_attack = require("config.unit.108_elf_archer.elf_archer_normal")
--this.normal_attack = require("config.unit.4_fire_apprentice.fire_apprentice_normal")
this.skills = {require("config.unit.4_fire_apprentice.fire_apprentice_skill1")}
this.passives = {}
this.battlecry = {}
this.deathrattle = {}


return this