local base = require("module.battle.skill.ripe_skill.base_skill")
local this = class("throw_skill",base)
local throw_vo = require("module.battle.skill.ripe_skill_vo.throw_skill_vo")

function this:ctor( vo,database )
    self.vo = vo
    self.database = database
    self.targets = {}
    --self:build_to_array("childs",vo.childs,database)
    self:build_to_array("effect",vo.effect,database)

    self.effect_entity = nil
    self.init = false
    self.speed = vo.speed
    self.min_dis = 9999

    self.start_pos = {X =0,Y =0,Z=0}
    self.curr_pos = {X =0,Y=0,Z=0}
    self.target_pos = {X = 0,Y = 0}
end

function this:execute( sess,delta )
    
    if self.init == false then
        if self.targets[1].alive ~= 0 then self:clean_up() return "completed" end
        self.effect_entity = self.effect[1]:execute(sess,self.targets[1])

        self.timepass = 0

        local x,y,z
        x,y,z = self.effect_entity:GetPos(x,y,z)
        self.start_pos.X = x
        self.start_pos.Y = y
        self.start_pos.Z = z
        self.curr_pos.X = x
        self.curr_pos.Y = y
        self.curr_pos.Z = z
        self.init = true
        
        if self.vo.trace == "curve" then
            if self.vo.target_type == throw_vo.Target.Unit and self.targets[1].alive == 0 then
                self.target_pos.X = self.targets[1].transform.grid_pos.X
                self.target_pos.Y = self.targets[1].transform.grid_pos.Y
            elseif self.vo.target_type == throw_vo.Target.Pos then
                self.target_pos.X = self.database.target_pos.X
                self.target_pos.Y = self.database.target_pos.Y
            end
            local Dx = self.target_pos.X - self.start_pos.X
            local Dy = self.target_pos.Y - self.start_pos.Y
            local all_dis = math.sqrt( Dx*Dx,Dy*Dy )
            self.curve_factor = all_dis
            self.curve_x = 0
            self.curve_z = 0
        end
    end

    if self["update_by_"..self.vo.trace](self,sess,delta) == "completed" then
        self:clean_up()
        return "completed"
    else
        return "running"
    end

end

function this:clean_up(  )
    self.effect[1]:clean_up()
end

function this:update_by_straight(sess,delta )
    if self.vo.target_type == throw_vo.Target.Unit and self.targets[1].alive == 0 then
        self.target_pos.X = self.targets[1].transform.grid_pos.X
        self.target_pos.Y = self.targets[1].transform.grid_pos.Y
    elseif self.vo.target_type == throw_vo.Target.Pos then
        self.target_pos.X = self.database.target_pos.X
        self.target_pos.Y = self.database.target_pos.Y
    end
    
    local de_x = self.target_pos.X - self.curr_pos.X
    local de_y = self.target_pos.Y - self.curr_pos.Y
    local de_z = 0.2 - self.curr_pos.Z
    local dis = math.sqrt( de_x*de_x+de_y*de_y )
    local time = dis/self.speed
    

    if dis > self.min_dis or time == 0 then return "completed"
    else self.min_dis = dis end
   
    de_x = de_x/time*delta
    de_y = de_y/time*delta
    de_z = de_z/time*delta

    
    self.curr_pos.X = self.curr_pos.X + de_x
    self.curr_pos.Y = self.curr_pos.Y + de_y
    self.curr_pos.Z = self.curr_pos.Z + de_z
    

    self.effect_entity:SetPos(self.curr_pos.X,self.curr_pos.Y,self.curr_pos.Z)

    self.speed = self.speed + delta*self.vo.a
    return "running"
    
end

function this:update_by_curve( sess,delta )
    if self.vo.target_type == throw_vo.Target.Unit and self.targets[1].alive == 0 then
        self.target_pos.X = self.targets[1].transform.grid_pos.X
        self.target_pos.Y = self.targets[1].transform.grid_pos.Y
    elseif self.vo.target_type == throw_vo.Target.Pos then
        self.target_pos.X = self.database.target_pos.X
        self.target_pos.Y = self.database.target_pos.Y
    end

    
    local de_x = self.target_pos.X - self.curr_pos.X
    local de_y = self.target_pos.Y - self.curr_pos.Y
    
    local dis = math.sqrt( de_x*de_x+de_y*de_y )
    local time = dis/self.speed
    
    if dis <= self.min_dis then self.min_dis = dis
    else return "completed" end

    de_x = de_x/time*delta
    de_y = de_y/time*delta

    local x,z = this.curve_z_calc(dis,self.curve_factor)
    if self.curve_x <= x then
        self.curve_z = z
        self.curve_x = x
    end

    self.curr_pos.X = self.curr_pos.X + de_x
    self.curr_pos.Y = self.curr_pos.Y + de_y
    self.curr_pos.Z = self.start_pos.Z + self.curve_z

    self.effect_entity:SetPos(self.curr_pos.X,self.curr_pos.Y,self.curr_pos.Z)

    return "running"
end

function this.curve_z_calc( rest_dis,all_dis )
    local x = math.max( 0,(1- rest_dis/all_dis))*2/3
    local z = -(x-1/3)*(x-1/3)+1/9
    z = z*all_dis/(2/3)/20
    return x,z
end

return this