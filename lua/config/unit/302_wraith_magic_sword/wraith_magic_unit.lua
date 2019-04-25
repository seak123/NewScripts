local this = {}

this.ai_vo = require("config.ai_config.normal_defence_ai")
this.normal_attack = require("config.skill_config.close_normal_attack")
this.skills = {}

this.sp_attr = {
    require("config.unit.302_wraith_magic_sword.wraith_magic_sp_attr")
}


return this