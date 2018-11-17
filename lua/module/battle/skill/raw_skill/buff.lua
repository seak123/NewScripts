local base = require("module.battle.skill.raw_skill.base_rawskill")
local this = class("buff",base)
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
    if self.vo.execute_type == 0 then _target = database.caster.unit 
    else _target = target end

    if _target.alive ~= 0 then return end
    if self:check(sess,database,_target) == false then
        return
    end

    local buffcont = _target.buffcont
    self:add_buff(sess,database,_target)
end

function this:add_buff( sess,database,target )
    print("add buff["..self.vo.buff_id.."] on "..target.data.name.." uid:"..target.uid)
    local inst = buff_inst.new(database,self.vo)
    local buff = target.buffcont:add_buff(sess,self.vo,inst)
end
  
return this
  
  
      