local base = require("module.battle.skill.raw_skill.base_rawskill")
local this = class("state",base)
local state_vo = require("module.battle.skill.raw_skill_vo.state_vo")


function this:ctor( vo,database )
    self.vo = vo
    self.database = database
    self:init_build(vo)
end

function this:execute(sess, target)
    local database = self.database
    if target.alive ~= 0 then return end
    if self:check(sess,database,target) == false then
        return
    end

   
end

function this:clean_up(  )
    -- if effect not auto_clean, nead clean manually
    if self.effect ~= nil then
        self.effect:CleanUp(self.vo.clean_delay)
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