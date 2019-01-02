
local this = {}

--------------battle def

---------------property
this.RATE = 0
this.VALUE = 1

this.PROPERTY = {
    hp = this.VALUE,
    attack = this.VALUE,
    attack_rate = this.RATE,
    defence = this.VALUE,
    magic_resist = this.RATE,
    crit = this.RATE,
    crit_value = this.RATE,
    hit_rate = this.RATE,
    dodge = this.RATE,
    speed = this.VALUE,
    physic_suck = this.RATE,
    magic_suck = this.RATE,
    coold_reduce = this.RATE
}

-- this.PLAYERPROP = {
--     hp = 0,
--     attack = 0,
--     defence = 0,
--     magic_resist = 0
-- }

this.PLAYER1POS = {
    X = 2,
    Y = 228
}

this.PLAYER2POS = {
    X = 798,
    Y = 228
}
-----------------field 
this.MAPMATRIX = {
    column = 800,
    row = 448
}

this.ENEMYBOUND = {
    column = 656,
    row = 448
}

this.MaxSpeed = 64
this.MinSpeed = 4
this.NormalSpeed = 16
this.DefaultSkyHurtZ = 3.5
this.DefaultGroundHurtZ = 0.75

this.MinAttackRate = 0.2

-----------------damage
this.DefenceFactor = 20

return this