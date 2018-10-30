local base = require("module.battle.skill.base_skill")
local this = class("fixed_skill",base)

function this:ctor( vo )
    self.vo = vo
    self:build_to_array("childs",self.subs)
    self.run_index = 1
end

function this:update( delta,database )
    while self.childs[self.run_index] ~= nil do
        local flag = self.childs[self.run_index]:update(delta,database)
        if flag == false then
            return false
        else
            self.run_index = self.run_index + 1
        end
    end
    return true        
end

return this