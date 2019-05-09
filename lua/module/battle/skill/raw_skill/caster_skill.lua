local base = require("module.battle.skill.raw_skill.base_rawskill")
local this = class("caster",base)
local entire_skill = require("module.battle.skill.entire_skill")
local pack = require("module.battle.skill.utils.pack_database")




function this:ctor( vo,database )
    self.vo = vo
    self.database = database
    self:init_build(vo)
end

function this:execute(sess, target)
    local database = self.database
    if target.alive ~= 0 then return end
    if self:check(sess,database,target) == false then
        return
    end
    local skill_vo = {
        root = {}
    }
    for _, v in ipairs(self.vo.skills) do
        table.insert( skill_vo.root, v)
    end
    local _target
    if self.vo.on_target == 1 then
        _target =  sess.field:get_unit(sess.trace:get_last_data().target_uid)
        if _target == nil then print("caster skill target is nil") return end
    else
        _target = sess.field:get_unit(sess.trace:get_last_data().caster_uid)
        if _target == nil then print("caster skill target is nil") return end
    end
    local skill = entire_skill.new(sess,skill_vo)
    local new_database = pack.pack_database(database.caster,_target,database.target_pos)
    skill:execute(new_database)
end

return this
  
  
      