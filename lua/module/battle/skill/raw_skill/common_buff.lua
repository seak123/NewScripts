local base = require("module.battle.skill.raw_skill.base_rawskill")
local this = class("common_buff",base)
local trace = require("module.battle.battle_trace")
local buff = require("module.battle.skill.raw_skill_vo.buff_vo")
local prop = require("module.battle.skill.raw_skill_vo.property_change_vo")
local calc = require("module.battle.skill.utils.caculate")
local buff_inst = require("module.battle.unit.component.buff.buff_inst")



function this:ctor( vo,database )
    self.vo = vo
    self.database = database
    self:init_build(vo)
end

function this:execute(sess, target)
    local database = self.database
    local _target = nil
    if self.vo.execute_type == 0 then _target = database.caster 
    else _target = target end

    if _target.alive ~= 0 then return end
    if self:check(sess,database,_target) == false then
        return
    end

    local buff_vo = this["get_buff_vo_"..self.vo.buff_type]()
    buff_vo.stack_num = self.vo.stack_num

    self:add_buff(sess,database,_target,buff_vo)

    self:execute_subs(sess,target)
end

function this:add_buff( sess,database,target,buff_vo)
    local inst = buff_inst.new(database,buff_vo)
    local buff = target.buffcont:add_buff(sess,buff_vo,inst)
end


----------------------------------- common buff vo ---------------------

function this.get_buff_vo_Poison (  )
    local buff_vo = buff.new()
    buff_vo.buff_id = 50
    buff_vo.duration = 2
    buff_vo.max_stack = -1
    buff_vo.update_type = 1
    --feature bit means:  0-benefit(0no1yes);2-dispel(0cannot1can);
    buff_vo.feature = 2
    buff_vo.execute_type = 1
    buff_vo.buff_type = buff.BuffType.Stack
    return buff_vo
end

function this.get_buff_vo_Rage(  )
    local attack_add = prop.new()
    attack_add.value_type = prop.ValueType.Override
    attack_add.prop_name = "attackadd"
    attack_add.calc = calc.make_buff_stack(1,0)
    attack_add.buff_occasion = "on_tick"

    local buff_vo = buff.new()
    buff_vo.buff_id = 51
    buff_vo.duration = -1
    buff_vo.max_stack = -1
    buff_vo.update_type = 1
    buff_vo.feature = 3
    buff_vo.execute_type = 1
    buff_vo.buff_type = buff.BuffType.Attr
    buff_vo.tick_occasion = "post_attack"
    buff_vo:append("belongs",attack_add)
    return buff_vo
end
  
return this
  
  
      