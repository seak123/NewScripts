local this = {}

function this.make_common_attack(rate, add) 
  return function(sess, caster, target) 
    return rate * caster.attack + add
  end
end

function this.make_rest_hp(rate,add_rate )
  return function (sess,caster,target  )
    local value = caster.hp_max * rate
    return math.floor( (caster.hp_max-caster.hp)/value )*add_rate
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
return this
