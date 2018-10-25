require("utils.functions")
local transform = require("module.battle.unit.component.transform")
local property = require("module.battle.unit.component.property")
local behavior_tree = require("module.battle.unit.behavior_tree.behavior_tree")
local bt_config = require("module.battle.unit.component.test_ai_config")

local unit = {}
unit.side = 1
print("start")
local betree = behavior_tree:build(unit,bt_config)

print("over")