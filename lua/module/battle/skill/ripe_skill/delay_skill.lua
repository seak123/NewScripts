local base = require("module.battle.skill.ripe_skill.base_skill")
local this = class("delay_skill",base)

function this:ctor( vo,database )
    self.vo = vo
    self.database = database
    self.time =0
    self.targets = {}
    --self:build_to_array("childs",vo.childs,database)
end

function this:execute( sess,delta )
    if self.targets[1]~= nil and self.targets[1].alive ~= 0 then return "completed" end
    self.time = self.time + delta
    if self.time > self.vo.delay then
        return "completed"
    else
        return "running"
    end
end

return this