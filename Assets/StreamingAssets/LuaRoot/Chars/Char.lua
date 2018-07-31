local Ctl = require "Chars/controller"
local Env = require "Chars/environment"

local M = {}

local function init(self, csObjChar, fsm)
    self.csObjChar = csObjChar
	self.env = Env.new(csObjChar)
	self.ctl = Ctl.new(csObjChar)
	self.fsm = fsm
	self.stateNo = 0
	self.stateTime = -1
end

function M.new(csObjChar, fsm)
    local obj = {}
	init(obj, csObjChar, fsm)
	setmetatable(obj, {__index = M})
	return obj
end

function M:update()
	if self.fsm == nil or self.fsm[self.stateNo] == nil then
		return
	end
    for i = -3, -1 do
    	if self.fsm[i] and self.fsm[i].onUpdate then
		    self.fsm[i].onUpdate(self)
	    end
    end
	self.stateTime = self.stateTime + 1
	if self.fsm[self.stateNo].onUpdate then
	    self.fsm[self.stateNo].onUpdate(self)
    else
    	print("stateNo:" .. self.stateNo .. " don't not have update Func")
    end
end

function M:changeState(stateNo)
	if self.fsm == nil or self.fsm[self.stateNo] == nil then
		print("don't have stateNo:" .. stateNo)
		return
	end
	if self.fsm[self.stateNo].onExit then
	    self.fsm[self.stateNo].onExit(self)
    else
    	print("stateNo:" .. self.stateNo .. "don't not have onExit Func")
    end
	self.stateNo = stateNo
	self.stateTime = -1
	if self.fsm[self.stateNo].onEnter then
	    self.fsm[self.stateNo].onEnter(self)
	else
    	print("stateNo:" .. self.stateNo .. "don't not have onEnter Func")
    end
end

local function toString(obj)
	if type(obj) == "number" then
	    return tostring(obj)
	elseif type(obj) == "userdata" then
		return obj:ToString()
	elseif type(obj) == "string" then
		return obj
	elseif type(obj) == "boolean" then
		if obj == true then
			return "true"
		else
			return "false"
		end
	else
		print(type(obj))
		return "nil"
	end
end

--[[
function M:debug()
	local guiDebug = CS.GUIDebug.Instance
	if guiDebug == nil then
		return
	end
	local index = self.player.slot
	guiDebug:AddMsg(index, "moveType", self.env:moveType())
    guiDebug:AddMsg(index, "physicsType", self.env:physicsType())
    guiDebug:AddMsg(index, "stateNo", toString(self.env:stateNo()))
    guiDebug:AddMsg(index, "stateTime", toString(self.env:stateTime()))
    guiDebug:AddMsg(index, "anim", toString(self.env:anim()))
    guiDebug:AddMsg(index, "animTime", toString(self.env:animTime()))
    guiDebug:AddMsg(index, "leftAnimTime", toString(self.env:leftAnimTime()))
    guiDebug:AddMsg(index, "animElem", toString(self.env:animElem())) 
    guiDebug:AddMsg(index, "animElemTime", toString(self.env:animElemTime()))
    guiDebug:AddMsg(index, "activeCommands", self.player.cmdMgr:GetActiveCommandName())
    guiDebug:AddMsg(index, "vel", toString(self.env:vel()))
end
--]]

return M