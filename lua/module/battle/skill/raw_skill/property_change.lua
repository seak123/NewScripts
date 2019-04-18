local base = require("module.battle.skill.raw_skill.base_rawskill")
local this = class("property_change",base)
local calc = require("module.battle.skill.utils.caculate")
local trace = require("module.battle.battle_trace")
local property_vo = require("module.battle.skill.raw_skill_vo.property_change_vo")


function this:ctor(vo,database)
    self.vo = vo
    self.database = database
	self:init_build(vo)
	self.value = 0
end

function this:execute(sess,target)
    local database = self.database
    local prop_name = self.vo.prop_name
    local change_value = self.vo.calc(self,sess,database.caster,target)

    if self.vo.value_type == property_vo.ValueType.Additive then
        self.value = self.value + change_value
    else
        local old = self.value
        self.value = change_value
        change_value = change_value - old
    end
    target.property:change_prop(prop_name,change_value)
end

function this:attach( buff )
    self.super.attach(self,buff)
    self.buff = buff
end

function this:detach( buff )
    self.super.detach(self,buff)
    buff.carrier.property:change_prop(self.vo.prop_name,-self.value)
end

return this