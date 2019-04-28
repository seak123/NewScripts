local this = {}
local damage_vo = require("module.battle.skill.raw_skill_vo.damage_vo")
local battle_def = require("module.battle.battle_def")

function this.make_common_attack(rate, add) 
  return function(self,sess, caster, target)
    if add == nil then add = 0 end
    return rate * caster.property:get("attack") + add
  end
end

function this.make_common_level(rate,add )
  return function(self,sess, caster, target)
    if add == nil then add = 0 end
    return rate * caster.level + add
  end
end

function this.make_rest_hp(rate,add_rate )
  return function (self,sess,caster,target  )
    local value = caster.hp_max * rate
    return math.floor( (caster.hp_max-caster.hp)/value )*add_rate
  end
end

function this.make_buff_stack( rate,add )
  return function (obj,sess,caster,target )
    local stack = obj.buff:get_inst_count()
    return rate*stack + add
  end
end

-- param: "caster", "field", rate... addtive
function this.make_common_calc(add, ...)
  local arr = {...}
  if arr == nil or #arr == 0 then
    return function() return add end
  else
    return function(sess, caster, target) 
      local res = add
      local i = 1
      while i < #arr do
        local utype = arr[i] -- "caster" : "target"
        local prop = arr[i+1]
        local rate = arr[i+2]
        local unit = caster
        if utype == "target" then
          unit = target
        end
        res = res + unit:get_calc_value(prop) * rate
        i = i + 3
      end
      return res
    end
  end
end

function this.make_increase_calc(base_rate,add_rate,add_value)
  if add_value == nil then add_value = 0 end
  return function (sess, caster, target)
  local trace = sess.trace
  local trace_skill = trace:last_of_type("trace_skill")
  local turns = trace_skill.turns 
  local rate = base_rate + turns * add_rate 
  return rate * caster:get_calc_value("attack") + add_value
end
end

function this.make_special_calc(command)
  local str = "function (sess, caster, target) \n return " .. command .. "\nend"
  local res = dostring(str)
  return res
end

function this.damage(caster,target,value,source,type )
  -- flag means:0,noraml attack 1, crit;2,miss;3,heal;
  local flag = 0;

  local crit_factor = 1
  if source == damage_vo.DamageSource.Attack then
    if math.random() > caster.property:get("hit_rate") and math.random() < target.property:get("dodge") then
      flag = 2
      value = 0
      return flag,value
    end
    
    if math.random() < caster.property:get("crit") then
      flag = 1
      crit_factor = caster.property:get("crit_value")
    end
  end
  if type == damage_vo.DamageType.Physical then
    local target_def = target.property:get("defence")
    local defence_factor = 1
    if target_def >= 0 then
      defence_factor = 1-target_def*battle_def.DefenceFactor/(100+target_def*battle_def.DefenceFactor)
    else
      defence_factor = 2-((100-battle_def.DefenceFactor)/100)^(-target_def)
    end
    value = value * defence_factor * crit_factor
    caster.sess.effect_mng:PrintMessage(target.uid,math.round(value),flag)
    return flag,value
  end
  if type == damage_vo.DamageType.Magic then
    local target_resist = target.property:get("magic_resist")
    target_resist = math.min( 1,target_resist )
    value = value * (1-target_resist)
    return flag,value
  end

  if type == damage_vo.DamageType.Real then
    return flag,value
  end

end

function this.heal( caster,target,value )
  caster.sess.effect_mng:PrintMessage(target.uid,math.round(value),3)
   return value
end

return this
