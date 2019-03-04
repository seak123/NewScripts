local base = require("module.battle.skill.raw_skill.base_rawskill")
local this = class("message",base)
local effect_vo = require("module.battle.skill.raw_skill_vo.effect_vo")


function this:ctor( vo,database )
    self.vo = vo
    self.text = vo.text
    self.database = database
    self:init_build(vo)
end

function this:execute(sess, target)
    local database = self.database
    sess.effect_mng:PrintMessage(database.caster.uid,self.text)
end

return this