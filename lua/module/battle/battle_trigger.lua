local this = quick_class("trigger_sys")


-- for triggers -3:world, -2:side 2, -1:side 1, >= 0 : single target uid


function this:ctor(sess)
	self.sess = sess
	self.queue_fore = {}
	self.queue_back = {}

	self.triggers = {}
end

function this:reg(item) 
  local source = item:get_source()
	local arr_tr = self.triggers[source]
	if arr_tr == nil then 
		arr_tr = {}
		self.triggers[source] = arr_tr
	end
	table.insert(arr_tr,item)
end

function this:unreg(item)
	-- body
  local source = item:get_source()
	local arr_tr = self.triggers[source]
	if arr_tr ~= nil then 
		for k,v in ipairs(arr_tr) do 
			if v == item then 
				table.remove(arr_tr, k)
				return 
			end
		end
	end
end

-- push player control to trigger system
function this:push_decide( caster, skill, aim )
	-- selector of target (passive state)
	local target = nil
	if caster.statectrl.confused == true then target = self.sess.field:get_alive_one(aim.side) else target = aim end
	table.insert(self.queue_back, function() skill:execute(self.sess, caster, target, true) end )
end

function this:handle_global_ev(event_name)
  return self:intn_handle_ev(event_name, -3, nil)
end

function this:handle_ev(event_name, target)
	-- body
  self:intn_handle_ev(event_name, target.uid, target)
  self:intn_handle_ev(event_name, -target.side, target)
end

-- handle event internal 
function this:intn_handle_ev(event_name, key, target)
  local arr_tr = self.triggers[key]
	if arr_tr ~= nil then 
		for _, v in ipairs(arr_tr) do 
			print("handle ev "..event_name)
			if v:check(self.sess, event_name, target) then
				print(target.name.." dispatch ["..event_name.."] in trigger_sys") 
				v:execute(self.sess, target)
			end
		end
	end
end

function this:push_executor( exec )
	print("push!!!!!!!!!!!")
	table.insert(self.queue_back, exec)
end

-- simulate the event queue and generate a perform timeline
function this:simulate()
	-- body
	self:swap() 
	
	-- local timeline = self.sess:get_timeline()
	--self.sess.timeline:push_queue()
	
	while #self.queue_fore > 0 do
		local len = #self.queue_fore
		for i=1,len do
			local func = self.queue_fore[i]
			self.queue_fore[i] = nil

			if type(func) == "function" then 
				func(self.sess)
			else 
				func:execute(self.sess)
			end
			self.sess.timeline:save_instance()
		end
		self:swap()
	end
	--self.sess.timeline:traceback()
end

function this:swap()
	local temp = self.queue_fore
	self.queue_fore = self.queue_back
	self.queue_back = temp
end

return this