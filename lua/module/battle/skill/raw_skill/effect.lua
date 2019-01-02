local base = require("module.battle.skill.raw_skill.base_rawskill")
local this = class("effect",base)
local effect_vo = require("module.battle.skill.raw_skill_vo.effect_vo")


function this:ctor( vo,database )
    self.vo = vo
    self.effect_id = vo.effect_id
    self.effect = nil
    self.database = database
    self:init_build(vo)
end

function this:execute(sess, target)
    local database = self.database
    if target.alive ~= 0 then return end
    if self:check(sess,database,target) == false then
        return
    end

    local pos = {X = 0, Y = 0}
    -- attach: means effect attach on gameobject
    local attach = false

    local effect = nil
    -- init pos
    if self.vo.execute_pos == effect_vo.ExecutePos.Caster then
        effect = sess.effect_mng:CreateEffect(self.effect_id,self.vo.attach,database.caster.unit.uid,0,0)
    elseif self.vo.execute_pos == effect_vo.ExecutePos.Target then
        effect = sess.effect_mng:CreateEffect(self.effect_id,self.vo.attach,target.uid,0,0)
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

function this:clean_up( uid )
    -- if effect not auto_clean, nead clean manually
    if uid == nil then uid = -1 end
    if self.effect ~= nil then
        self.effect:CleanUp(self.vo.clean_delay,uid)
    end
end

function this:attach( buff )
    self.super.attach(self,buff)
    self.buff = buff
end

function this:detach( buff )
    self.super.detach(self,buff)
    self:clean_up()
end

return this