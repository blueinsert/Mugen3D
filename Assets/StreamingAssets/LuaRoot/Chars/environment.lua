local trigger = require "trigger.cs"

local M = {}

function M.new(charFsm)
    local obj = {}
    obj.charFsm = charFsm
	setmetatable(obj, {__index = M})
	return obj
end

function M:CommandTest(commandName)
   local res = trigger.CommandTest(self.charFsm.csObjChar, commandName)
   --print("commandTest " .. commandName .. ":" .. res)
   return res
end

function M:AttackCheck()
    local res = trigger.AttackCheck(self.charFsm.csObjChar)
    return res
end

function M:Facing()
    return trigger.Facing(self.charFsm.csObjChar)
end

function M:MoveType()
    return trigger.MoveType(self.charFsm.csObjChar)
end

function M:PhysicsType()
    return trigger.PhysicsType(self.charFsm.csObjChar)
end

function  M:JustOnGround()
    return trigger.JustOnGround(self.charFsm.csObjChar)
end

function M:StateNo()
    return self.charFsm.stateNo
	--return trigger.StateNo(self.charFsm.csObjChar)
end

function M:StateTime()
    return self.charFsm.stateTime
    --return trigger.StateTime(self.charFsm.csObjChar)
end

function M:Anim()
    return trigger.Anim(self.charFsm.csObjChar)
end

function M:AnimExist(anim)
    return trigger.AnimExist(self.charFsm.csObjChar, anim)
end

function M:AnimTime()
    return trigger.AnimTime(self.charFsm.csObjChar)
end

function M:AnimElem()
    return trigger.AnimElem(self.charFsm.csObjChar)
end

function M:AnimElemTime()
    return trigger.AnimElemTime(self.charFsm.csObjChar)
end

function M:LeftAnimTime()
    return trigger.LeftAnimTime(self.charFsm.csObjChar)
end

function M:Vel()
    local x,y = trigger.Vel(self.charFsm.csObjChar)
    return {x = x, y = y}
end

function M:Pos()
    local x,y = trigger.Pos(self.charFsm.csObjChar)
    return {x = x, y = y}
end

return M