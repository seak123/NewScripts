local this = class("buff_container")

function this:ctor( master )
    self.buffs = {}
    self.master = master
end

function this:update( delta )
    for _, b in ipairs(self.buffs) do
        b:update(delta)
    end
end

function this:add_buff(sess, buff_vo, inst)
    print(">>>>>>>>>>>>>>>>>>>>>>>>")
      local buff_id = buff_vo.buff_id
      local conf = config[buff_id]
      local buff = nil
      print(inst.caster.name)
      local is_new = true
      buff = self.buffs[buff_id]
      if conf.buff_type == STABLE_BUFF then
          buff = stable.new(buff_vo.buff_id, buff_vo)
          buff.uid = inst.uid
          buff:attach(sess, self.unit)
          self.buffs[buff:get_key()] = buff
      elseif conf.buff_type == SIMPLE_BUFF then
          if buff ~= nil then
              self:remove_buff(sess,buff,true)
          end
          buff = simple.new(buff_vo.buff_id, buff_vo)
          buff.uid = inst.uid
          buff:attach(sess, self.unit)
          self.buffs[buff:get_key()] = buff
      elseif conf.buff_type == STACK_BUFF then
          if buff == nil then
              buff = stack.new(buff_vo.buff_id, buff_vo)
              buff.uid = inst.uid
              buff:attach(sess, self.unit)
              self.buffs[buff:get_key()] = buff
          else
              is_new = false
          end
      end
      buff:handle_stack(sess, inst)
      if is_new then
          self:inform_new_buff(sess,buff,true)
      else
          self:inform_view(sess)
      end
      return buff
  end
  
  function this:remove_buff(sess, buff, is_move)
      buff:remove(sess, is_move)
      if is_move then
          buff:detach(sess, self.unit)
          self.buffs[buff:get_key()] = nil
      end
  end
  
  function this:remove_buff_by_id( sess,buff_id,is_move )
  
      for i,v in pairs(self.buffs) do
          print("------------------"..i..v:get_key())
      end
      local buff = self.buffs[buff_id]
      if buff ~= nil then buff:remove(sess,is_move) end
      if is_move then
          buff:detach(sess,self.unit)
          self.buffs[buff_id] = nil
      end
  end
  
  function this:find_buff(key)
      return self.buffs[key]
  end
  
  function this:find_first_if(pred)
      for _, v in pairs(self.buffs) do
          if not v.is_remove and pred(v) then 
              return v
          end
      end
      return nil
  end
  
  function this:find_all_if(pred)
      local res = {}
      for _, v in pairs(self.buffs) do
          if not v.is_remove and pred(v) then 
              table.insert(res, v)
          end
      end
      return res
  end
  
  function this:find_one_if(pred)
      local arr = self:find_all_if(pred)
      if #arr == 0 then 
          return nil
      else
          local index = math.random(#arr)
          return arr[index]
      end
  end
  
  function this:extend_buff(sess, buff, extend_turns)
      local inst = buff:get_inst()
      inst:change_timepass(sess,extend_turns)
  end
  
  function this:move_stack(sess, other, buff)
      local conf = buff.config
      if buff:get_stack_count() > 1 then
          -- get random inst
          local inst = buff:get_inst()
          buff:remove_inst(inst)
          self:add_buff(sess, buff.vo, inst)
      else
          return self:move_buff(sess, other, buff)
      end
  end
  
  -- other : other buff container
  function this:move_buff(sess, other, buff)
      other:remove_buff(sess, buff, true)
      buff:attach(sess,self.unit)
      self.buffs[buff.vo.buff_id] = buff
      self:inform_new_buff(sess, buff,true)
  end
  
  function this:remove_all(sess)
      for _, v in pairs(self.buffs) do
          if not v.is_remove then
              --inform_new_buff in remove func
              v:remove(sess)
          end
          v:detach(sess, self.unit)
      end
      self.buffs = {}
  end
  
  
  function this:handle(sess, event_name)
      -- body
      for _, v in pairs(self.buffs) do
          if not v.is_remove then 
              v:handle(sess, event_name)
          end
      end
      --return self:clear_removed(sess)
  end
  
  function this:inform_view(sess)
      for _,v in pairs(self.buffs) do
      local buff = v
      
      if buff:get_turn_left() ~= -1 and not buff.is_remove then
      local node_vo = {}
      node_vo.carrier_uid = buff.carrier.uid
      node_vo.buff_uid = buff.uid
      node_vo.buff_id = buff.vo.buff_id
      node_vo.stack_num = buff:get_inst_count()
      node_vo.unit_type = buff.vo.unit_type
      node_vo.fx_id = config[buff.vo.buff_id].fx_id
      node_vo.longest_time = buff:get_turn_left()
      node_vo.refresh_type = 1
  --[[    node_vo.unit_type = buff.vo.unit_type
      node_vo.mul_fx = buff.vo.mul_fx
      node_vo.asset = buff.vo.buff_fx_asset
      node_vo.attach_point = buff.vo.fx_attach_point
      node_vo.longest_time = self:get_longest_time(buff.vo)]]
      local node = buff_node.new(node_vo)
      sess.timeline:push(node)
      sess.timeline:traceback()
      --sess.timeline:traceback()
      end
      end
  end
  
  function this:inform_new_buff(sess,buff,is_add)
      if buff:get_turn_left() == -1 or buff.is_remove then return end
      local node_vo = {}
      node_vo.carrier_uid = buff.carrier.uid
      node_vo.buff_uid = buff.uid
      node_vo.buff_id = buff.vo.buff_id 
      node_vo.unit_type = buff.vo.unit_type
      node_vo.fx_id = config[buff.vo.buff_id].fx_id
      if is_add == true then
          node_vo.stack_num = buff:get_inst_count()
          node_vo.longest_time = buff:get_turn_left()
          node_vo.refresh_type = 0
      else
          node_vo.stack_num = 0
          node_vo.longest_time = 0
          node_vo.refresh_type = 2
          -- local remove_id = config[buff.vo.buff_id].remove_fxid
          -- if remove_id ~= nil and  remove_id ~= 0 then
          --     local fx_vo = {
          --         id = remove_id,
          --         perform_type = 0,
          --         duration = 0,
          --         unit_type = buff.carrier.type,
          --         interp_delay = 0
          --     }
          --     local fx_node = fx_node.new(buff.carrier,buff.carrier,fx_vo)
          --     sess.timeline:push(fx_node)
          --     sess.timeline:traceback()
          -- end
      end
      local node = buff_node.new(node_vo)
      sess.timeline:push(node)
      sess.timeline:traceback()
  end
  
  return this