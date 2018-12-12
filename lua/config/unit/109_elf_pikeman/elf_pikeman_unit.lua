local this = {}

this.ai_vo = require("config.ai_config.normal_ai")
this.normal_attack = require("config.skill_config.close_normal_attack")
this.skills = {}
this.passives = {require("config.unit.109_elf_pikeman.elf_pikeman_passive1")}
this.battlecry = {}
this.deathrattle = {}


return this