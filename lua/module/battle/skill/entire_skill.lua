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
        table.insert( skill.targets, database.target.unit)
        table.insert( self.playing, skill )
    end
    self.sess.skill_mng:reg(self)
end

function this:update( delta )
    if #self.playing == 0 then
        return true
    end
    for index=#self.playing,1,-1 do
        local flag = self.playing[index]:execute(self.sess,delta)
        if flag == "completed" then
            -- skill completed, then execute its child-node
            local targets = self.playing[index].targets
            if #targets > 0 then
                if #targets >1 then print("@@targe1 "..targets[1].uid) print("@@target2 "..targets[2].uid) end
                -- check targets, muti target means execute more times
                for _,unit in ipairs(targets) do
                    if self.playing[index].vo.childs ~= nil then
                        local subs = self.playing[index].vo.childs
                        for _, v in ipairs(subs) do
                            local temp = require(v.execute)
                            if temp ~= nil then
                                local exec = temp.new(v,self.playing[index].database)
                                print("@@insert playing "..unit.uid)
                                table.insert( self.playing, exec)
                                table.insert( exec.targets, unit)
                            end
                        end
                    end
                end
            end
            table.remove( self.playing,index )
        elseif flag == "failure" then
            return true
        end
    end
    return false
end


return this