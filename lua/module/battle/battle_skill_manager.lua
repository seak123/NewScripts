local this = class("battle_skill_manager")

function this:ctor( sess )
    self.sess = sess
    self.play_list = {}
end

function this:reg( skill )
    table.insert( self.play_list, skill)
end

function this:update( delta )
    for index=#self.play_list,1,-1 do
        if self.play_list[index]:update(delta) == true then
            table.remove( self.play_list, index )
        end
    end
end

return this