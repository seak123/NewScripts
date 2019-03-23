local base = require("module.battle.skill.raw_skill.base_rawskill")
local this = class("common_buff",base)
local trace = require("module.battle.battle_trace")
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

    for i=1,self.vo.stack_num do
        self:add_buff(sess,database,_target,buff_vo)
    end
    self:execute_subs(sess,target)
end

function this:add_buff( sess,database,target,buff_vo)
    local inst = buff_inst.new(database,buff_vo)
    local buff = target.buffcont:add_buff(sess,buff_vo,inst)
end


----------------------------------- common buff vo ---------------------

function this.get_buff_vo_Poison (  )
    local buff_vo = {}
    buff_vo.buff_id = 50
    buff_vo.duration = 2
    buff_vo.max_stack = -1
    buff_vo.update_type = 1
    --feature bit means:  0-benefit(0no1yes);2-dispel(0cannot1can);
    buff_vo.feature = 2
    buff_vo.belongs = {}
    return buff_vo
end
  
return this
  
  
      