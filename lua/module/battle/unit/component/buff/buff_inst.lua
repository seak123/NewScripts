local this = class("buff_inst")

function this:ctor(caster,duration )
    self.caster = caster
    self.duration = duration
    self.timepass = 0
    self.is_expire = false
end

function this:update( delta )
    self.timepass = self.timepass + delta
    if self.timepass > self.duration then
        self.is_expire = true
    end
end