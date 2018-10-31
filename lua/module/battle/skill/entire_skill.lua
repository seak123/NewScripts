local this = class("entire_skill")

function this:ctor( sess,skill_vo)
    self.sess = sess
    self.vo = skill_vo
end

function this:execute(database )
    self.database = database
    self.playing = {}
    for _,v in ipairs(self.vo.root) do
        local skill = require(v.execute).new(v,self.database)
        table.insert( self.playing, skill )
    end
    self.sess.skill_mng:reg(self)
end

function this:update( delta )
    if #self.playing == 0 then
        return true
    end
    for index=#self.playing,1,-1 do
        if self.playing[index]:execute(self.sess,delta) == true then
            if self.playing[index].childs ~= nil then
                local subs = self.playing[index].childs
                for _, v in ipairs(subs) do
                    table.insert( self.playing, v)
                end
            end
            table.remove( self.playing,index )
        end
    end
    return false
end

return this