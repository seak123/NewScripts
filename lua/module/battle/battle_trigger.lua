local this =class("battle_trigger")


-- for triggers -3:world, -2:side 2, -1:side 1, >= 0 : single target uid


function this:ctor(sess)
	self.sess = sess
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

function this:handle_global_ev(event_name)
  return self:intn_handle_ev(event_name, -3, nil)
end

function this:handle_ev(event_name, target)
	-- trigger unit event
	self:intn_handle_ev(event_name, target.uid, target)
	-- trigger room event
	self:intn_handle_ev(event_name, -target.location, target)
	-- trigger other event
end

-- handle event internal 
function this:intn_handle_ev(event_name, key, target)
  local arr_tr = self.triggers[key]
	if arr_tr ~= nil then 
		for _, v in ipairs(arr_tr) do 
			if v:check(self.sess, event_name, target) then
				v:execute(self.sess, target)
			end
		end
	end
end


return this