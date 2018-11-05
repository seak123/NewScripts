local base = require("module.battle.skill.raw_skill.base_rawskill")
local this = class("buff",base)
local trace = require("module.battle.battle_trace")



function this:ctor( vo )
    self.vo = vo
    self:init_build(vo)
end

function this:execute(sess, delta ,database,target)
    local _target = nil
    if self.vo.execute_type == 0 then _target = database.caster.unit 
    else _target = target end

    if _target.alive ~= 0 then return end
    if self:check(sess,database,_target) == false then
        return
    end

    local buffcont = _target.buffcont
    self:add_buff(sess,database.caster.unit,_target)
end

function this:add_buff( sess,caster,target )
    local inst = buff_inst.new()
end

function this:add_buff(sess, caster, target)
    for index=1,self.vo.stack_num do
      
    local inst = buff_inst.new(caster, self.vo.duration)
    if inst.duration < 0 then 
      print(self.vo.buff_id)
    end
    inst.uid = this.uid
    this.uid = this.uid + 1
    if caster == target and self.vo.turn_point == buff_vo.ON_TURN then
      inst.skip = 1
    end
      local buff = target.buffcont:add_buff(sess, self.vo, inst)
    
    sess.logger:log("[BUFF] "..caster:full_name().." add buff id "..self.vo.buff_id.." to "..target:full_name()..",name "..buff.config.name..",left "..buff:get_turn_left().." turns, stacks ".. buff:get_inst_count() )
    
  --[[	
      local buff = buff_effect.new(self.vo)
      this.uid = this.uid + 1
      buff.uid = this.uid
      buff:setup(sess, caster, target)
      target.buffcont:add_buff(sess, buff)
  ]]    	
  --	local sk = sess.trace:last_of_type("trace_skill")
  --    buff.trace_skill = sk
    buff:push_buff(sess)
    self:call_for_one("subs", sess, caster, target)
    buff:pop_buff(sess)
    end
  
  end
  
  function this:execute_one(sess, caster, target)
      -- print("execute buff ["..self.vo.name.."] effect on "..target.name.." from "..caster.name)
  
      if self:check(sess,caster,target) == false then return end
  
      local buffcont = target.buffcont
      if buffcont == nil then 
          return 
      end
        sess.timeline:push_empty()
      self:add_buff(sess,caster,target)
      return sess.timeline:traceback()
  end
  
  function this:push_buff(sess)
    local trace = sess.trace
    local tr = {
      type_name = "trace_buff",
      buff = self,
      caster = self:get_caster()
    }
    trace:push(tr)
  end
  
  function this:pop_buff(sess)
    local trace = sess.trace
    trace:pop()
  end
  
  --[[
  function this:load_config( id )
      local config = buff_mng.get_buff_vo(id)
      if config == nil then return end
  
      self.vo.name = config.name
      self.vo.stack_type = config.stack_type
      self.vo.turn_point = config.turn_point
      self.vo.fx_id = config.fx_id
      self.vo.icon_id = config.icon_id
      self.vo.description = config.description
      self.vo.stack_max = config.stack_max
      self.vo.interval = config.interval
  end
  ]]
  return this
  
  
      