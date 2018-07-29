local M = {}

function M.new(char)
    local obj = {}
    obj.char = char
	setmetatable(obj, {__index = M})
	return obj
end

function M:CommandTest(commandName)
   return self.char.player.cmdMgr:CommandIsActive(commandName)
end

function M:moveType()
    return self.char.player.status.moveType:ToString()
end

function M:physicsType()
    return self.char.player.status.physicsType:ToString()
end

function  M:justOnGround()
    return self.char.player.moveCtr.justOnGround
end

function M:stateNo()
	return self.char.stateNo
end

function M:stateTime()
    return self.char.stateTime
end

function M:anim()
    return self.char.player.animCtr.anim
end

function M:animTime()
    return self.char.player.animCtr.animTime
end

function M:animElem()
    return self.char.player.animCtr.animElem
end

function M:animElemTime()
    return self.char.player.animCtr.animElemTime
end

function M:leftAnimTime()
    return self.char.player.animCtr.animLength - self:animTime()
end

function M:vel()
    return self.char.player.moveCtr.velocity
end

return M