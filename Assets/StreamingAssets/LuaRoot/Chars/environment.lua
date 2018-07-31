local trigger = require "trigger.cs"

local M = {}

function M.new(csObjChar)
    local obj = {}
    obj.csObjChar = csObjChar
	setmetatable(obj, {__index = M})
	return obj
end

function M:CommandTest(commandName)
   local res = trigger.CommandTest(self.csObjChar, commandName)
   --print("commandTest " .. commandName .. ":" .. res)
   return res
end

function M:MoveType()
    return trigger.MoveType(self.csObjChar)
end

function M:PhysicsType()
    return trigger.PhysicsType(self.csObjChar)
end

function  M:JustOnGround()
    return trigger.JustOnGround(self.csObjChar)
end

function M:StateNo()
	return trigger.StateNo(self.csObjChar)
end

function M:StateTime()
    return trigger.StateTime(self.csObjChar)
end

function M:Anim()
    return trigger.Anim(self.csObjChar)
end

function M:AnimTime()
    return trigger.AnimTime(self.csObjChar)
end

function M:AnimElem()
    return trigger.AnimElem(self.csObjChar)
end

function M:AnimElemTime()
    return trigger.AnimElemTime(self.csObjChar)
end

function M:LeftAnimTime()
    return trigger.LeftAnimTime(self.csObjChar)
end

function M:Vel()
    return trigger.Vel(self.csObjChar)
end

return M