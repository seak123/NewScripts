--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2018-10-13 10:55:32
]]
local this = {}

this.RATE = 0
this.VALUE = 1

this.PROPERTY = {
    hp = this.VALUE,
    attack = this.VALUE,
    attack_rate = this.RATE,
    defence = this.VALUE,
    crit = this.RATE,
    speed = this.VALUE
}

this.MAPMATRIX = {
    column = 1152,
    row = 448
}

this.MaxSpeed = 32
this.MinSpeed = 4
this.NormalSpeed = 16

return this