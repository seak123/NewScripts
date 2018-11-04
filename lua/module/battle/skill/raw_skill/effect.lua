local base = require("module.battle.skill.raw_skill.base_rawskill")
local this = class("effect",base)
local effect_vo = require("module.battle.skill.raw_skill_vo.effect_vo")


function this:ctor( vo )
    self.vo = vo
    self.effect_id = vo.effect_id
    self.effect = nil
    self:init_build(vo)
end

function this:execute(sess, delta ,database,target)
    local pos = {X = 0, Y = 0}
    -- attach: means effect attach on gameobject
    local attach = false

    local effect = nil
    -- init pos
    if self.vo.execute_pos == effect_vo.ExecutePos.Caster then
        print("@@ effecid:"..self.effect_id.."attach"..tostring(self.vo.attach).."uid"..database.caster.unit.uid)
        effect = sess.effect_mng:CreateEffect(self.effect_id,self.vo.attach,database.caster.unit.uid,0,0)
    elseif self.vo.execute_pos == effect_vo.ExecutePos.Target then
        effect = sess.effect_mng:CreateEffect(self.effect_id,self.vo.attach,database.target.uid,0,0)
    elseif self.vo.execute_pos == effect_vo.ExecutePos.CasterPos then
        pos.X = database.caster_pos.X
        pos.Y = database.caster_pos.Y
        effect = sess.effect_mng:CreateEffect(self.effect_id,false,-1,pos.X,pos.Y)
    elseif self.vo.execute_pos == effect_vo.ExecutePos.TargetPos then
        pos.X = database.target_pos.X
        pos.Y = database.target_pos.Y
        effect = sess.effect_mng:CreateEffect(self.effect_id,false,-1,pos.X,pos.Y)
    end

    if effect ~= nil then self.effect = effect end

    return effect
end

function this:clean_up(  )
    -- if effect not auto_clean, nead clean manually
    self.effect:CleanUp()
end

return this