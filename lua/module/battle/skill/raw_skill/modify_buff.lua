local base = require("module.battle.skill.raw_skill.base_rawskill")
local this = class("modify_buff",base)
local calc = require("module.battle.skill.utils.caculate")
local trace = require("module.battle.battle_trace")

function this:ctor( vo,database )
    self.vo = vo
    self.database = database
    self:init_build(vo)
end

function this:execute(sess, target)
    -- base execute
    if target.alive ~= 0 then return end
    if self:check(sess,self.database,target) == false then
        return
    end

    local database = self.database
    self["execute_by_"..self.vo.mdf_type](self,sess,target)
end

function this:execute_by_dispel( sess,target )
    
end

function this.execute_by_destroy( sess,target )
    
end

function this:attach( buff )
    self.super.attach(self,buff)
    self.buff = buff
end

function this:detach( buff )
    self.super.detach(self,buff)
end

return this