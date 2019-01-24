local this = class("buff_inst")

function this:ctor(database,buff_vo )
    self.database = database
    self.duration = buff_vo.duration
    self.timepass = 0
    self.tick_cache = 0
    self.is_expire = false
    self.belongs = {}

    if buff_vo.belongs ~= nil then
        for _,v in ipairs(buff_vo.belongs) do
            local clazz = require(v.execute)
            local item = clazz.new(v,database)
            table.insert( self.belongs, item)
        end
    end
end

function this:attach(sess)
	for _, v in ipairs(self.belongs) do
        v:attach(self.buff)
        if v.vo.buff_occasion == "on_add" then
            v:execute(sess,self.buff.carrier)
        end
	end
end

function this:detach(sess)
    for _, v in ipairs(self.belongs) do
        if v.vo.buff_occasion == "on_remove" then
            v:execute(sess,self.buff.carrier)
        end
		v:detach(self.buff)
	end
end

function this:update( delta )
    if self.duration <0 then return end
    self.tick_cache = self.tick_cache + delta
    if self.tick_cache > 1 then
    for _,v in ipairs(self.belongs) do
        if v.vo.buff_occasion == "on_tick" then
        end
    end
end
    self.timepass = self.timepass + delta
    if self.timepass > self.duration then
        self.is_expire = true
    end
end

return this