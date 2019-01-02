
local this = class("property")
local def = require("module.battle.battle_def")

function this:ctor( master,prop )
    self:init()
    self.master = master
    for n,v in pairs(prop) do
        self[n.."base"] = v
    end
end

function this:init(  )
    for n,v in pairs(def.PROPERTY) do
        if v == def.RATE then
            self[n.."base"] = 0
            self[n.."add"] = 0 
        else
            self[n.."base"] = 0
            self[n.."rate"] = 0
            self[n.."add"] = 0
        end
        self[n] = 0
    end
end

function this:change_prop( name,value )
    print("change value"..name..value.."on"..self.master.uid)
    self[name] = self[name] + value
end

function this:get( name )
    if def.PROPERTY[name] == def.RATE then
        return self[name.."base"] + self[name.."add"]
    else
        return self[name.."base"]*(1+self[name.."rate"]) + self[name.."add"]
    end 
end

function this.unpack_prop( data )
    local prop_def = require("module.battle.battle_def").PROPERTY
    local prop = {}
    for n,_ in pairs(prop_def) do
        prop[n] = data[n]
    end
    return prop
end

return this