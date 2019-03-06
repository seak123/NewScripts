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
    local skill_vo = {
        root = {}
    }
    for _, v in ipairs(self.vo.skills) do
        table.insert( skill_vo.root, v)
    end
    local skill = entire_skill.new(sess,skill_vo)
    local new_database = pack.pack_database(database.caster,target,database.target_pos)
    skill:execute(new_database)
end

return this
  
  
      