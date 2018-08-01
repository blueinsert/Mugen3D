local controller = require "controller.cs"

local M = {}

function M.new(charFsm)
    local obj = {}
	obj.charFsm = charFsm
	setmetatable(obj, {__index = M})
	return obj
end

function M:ChangeState(stateNo)
    self.charFsm:changeState(stateNo)
    --controller:ChangeState(self.charFsm.csObjChar, stateNo)
end

function M:ChangeAnim(animNo)
    controller.ChangeAnim(self.charFsm.csObjChar, animNo)
end

function M:PhysicsSet(physics)
    controller.PhysicsSet(self.charFsm.csObjChar, physics)
end

function M:MoveTypeSet(moveTypeStr) 
   controller.MoveTypeSet(self.charFsm.csObjChar, moveTypeStr)
end

function M:VelSet(x, y)
    controller.VelSet(self.charFsm.csObjChar, x, y)
end 

function M:CtrlSet(canCtrl)
    controller.CtrlSet(self.charFsm.csObjChar, canCtrl)
end

function M:Pause(duration)
    controller.Pause(self.charFsm.csObjChar, duration)
end

return M