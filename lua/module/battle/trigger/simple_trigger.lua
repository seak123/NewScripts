local base = require("module.battle.trigger.base_trigger")
local this = class("simple_trigger", base)
local pack_data = require("module.battle.skill.utils.pack_database")

function this:ctor(vo, owner)
  self.vo = vo
  --self.limit = vo.limit
	self.occasion = vo.occasion
	self.checkers = vo.checkers
	self.owner = owner
  self.source = vo.source
  --self.owner_type = vo.owner_type
	self.target_type = vo.target_type
  self.root = vo.root

  
  
end

function this:execute(sess, target)
  if self.target_type == "friend" then
    if self.owner.side ~= target.side then return end
  elseif self.target_type == "enemy" then
    if self.owner.side == target.side then return end
  end
  print("@@simple trigger attach!:"..target.uid)
  self.database = pack_data.pack_database(self.owner,target,target.transform.grid_pos)
  local raw_skill = require(self.root.execute).new(self.root,self.database)
  raw_skill:execute(sess,target)
end

return this