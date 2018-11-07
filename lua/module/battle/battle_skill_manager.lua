local this = class("battle_skill_manager")
local skill_config= require("config.config_manager")
local entire_skill = require("module.battle.skill.entire_skill")
local pack = require("module.battle.skill.utils.pack_database")

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

function this:caster_skill( side,skill_id,pos_x,pos_y )
    local skill_vo = skill_config.get_skill_config(skill_id)
    local skill = entire_skill.new(self.sess,skill_vo)
    local database = pack.pack_common_database( self.sess,side,{X= pos_x,Y = pos_y} )
    skill:execute(database)
end

return this