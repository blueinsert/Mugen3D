local controller = require "controller.cs"

local M = {}

function M.new(csObjChar)
    local obj = {}
	obj.csObjChar = csObjChar
	setmetatable(obj, {__index = M})
	return obj
end

function M:ChangeState(stateNo)
    controller:ChangeState(self.csObjChar, stateNo)
end

function M:ChangeAnim(animNo)
    controller.ChangeAnim(self.csObjChar, animNo)
end

function M:PhysicsSet(physics)
    controller.PhysicsSet(self.csObjChar, physics)
end

function M:MoveTypeSet(moveTypeStr) 
   controller.MoveTypeSet(self.csObjChar, moveTypeStr)
end

function M:VelSet(x, y)
    controller.VelSet(self.csObjChar, x, y)
end 

function M:CtrlSet(canCtrl)
    controller.CtrlSet(self.csObjChar, canCtrl)
end

function M:Pause(duration)
    controller.Pause(self.csObjChar, duration)
end

return M