local this = class("buff_container")
local vo_buff = require("module.battle.skill.raw_skill_vo.buff_vo")
local stack = require("module.battle.unit.component.buff.stack_buff")
local attr = require("module.battle.unit.component.buff.attr_buff")

function this:ctor( master )
    self.buffs = {}
    self.master = master
    self.sess = master.sess
end

function this:update( delta )
    for _, b in pairs(self.buffs) do
        b:update(delta)
    end
    self:clear_remove(self.sess)
end

function this:add_buff(sess, buff_vo, inst)
    local buff_id = buff_vo.buff_id
    local buff = nil

    buff = self.buffs[buff_id]

    if buff == nil then
        if buff_vo.buff_type == vo_buff.BuffType.Stack then
            buff = stack.new(sess,buff_vo.buff_id,buff_vo)
        elseif buff_vo.buff_type == vo_buff.BuffType.Attr then
            buff = attr.new(sess,buff_vo.buff_id,buff_vo)
        end
        buff:attach_buff(sess, self.master)
        self.buffs[buff:get_key()] = buff
    end
    buff:handle_stack(sess,inst)
    return buff
end

function this:clear_remove( sess )
    for _, b in ipairs(self.buffs) do
        if b.is_remove then
            self:remove_buff(sess,b)
        end
    end
end
  
function this:remove_buff(sess, buff)
    buff:remove(sess)

    buff:detach_buff(sess, self.master)
    self.buffs[buff:get_key()] = nil
end
  
function this:remove_buff_by_id( sess,buff_id )

    local buff = self.buffs[buff_id]
    if buff ~= nil then buff:remove(sess)
        buff:detach_buff(sess,self.unit)
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

  
  -- other : other buff container
function this:move_buff(sess, other, buff)
    other:remove_buff(sess, buff)
    buff:attach_buff(sess,self.unit)
    self.buffs[buff.vo.buff_id] = buff
end
  
function this:remove_all(sess)
    for _, v in pairs(self.buffs) do
        if not v.is_remove then
            --inform_new_buff in remove func
            v:remove(sess)
        end
        v:detach_buff(sess, self.unit)
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
end
  
return this