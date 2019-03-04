local this = {}

this.attr_config = {
    [11] = require("config.attr_config.1_wind_rage.11_wind_rage")
}

function this.get_attr_config( id )
    return this.attr_config[id]
end


return this