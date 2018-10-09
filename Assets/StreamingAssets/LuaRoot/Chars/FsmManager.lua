local controller = require "controller.cs"
local trigger = require "trigger.cs"

local controllers = {
	{name = "ChangeAnim",   func = controller.ChangeAnim},
	{name = "ChangeFacing", func = controller.ChangeFacing},
	{name = "ChangeState",  func = controller.ChangeState},
	{name = "CtrlSet",      func = controller.CtrlSet},
	{name = "HitDefSet",    func = controller.HitDefSet},
	{name = "MoveTypeSet",  func = controller.MoveTypeSet},
	{name = "PhysicsSet",   func = controller.PhysicsSet},
	{name = "VelSet",       func = controller.VelSet},
	{name = "Pause",        func = controller.Pause},
}

local triggers = {
	{name = "CommandTest",  func = trigger.CommandTest},
	{name = "Facing",       func = trigger.Facing},
	{name = "MoveType",     func = trigger.MoveType},
	{name = "PhysicsType",  func = trigger.PhysicsType},
	{name = "StateNo",      func = trigger.StateNo},
	{name = "StateTime",    func = trigger.StateTime},
	{name = "Anim",         func = trigger.Anim},
	{name = "AnimExist",    func = trigger.AnimExist},
	{name = "AnimTime",     func = trigger.AnimTime},
	{name = "AnimElem",     func = trigger.AnimElem},
	{name = "AnimElemTime", func = trigger.AnimElemTime},
	{name = "LeftAnimTime", func = trigger.LeftAnimTime},
	{name = "Vel",          func =  function(...)
        local x,y = trigger.Vel(...)
		return {x = x, y = y}
	end},
	{name = "Pos",          func = trigger.Pos},
	{name = "JustOnGround", func = trigger.JustOnGround},
	{name = "P2Dist",       func =  function(...)
		local x,y = trigger.P2Dist(...)
		return {x = x, y = y}
	end},
	{name = "GetHitVar",    func = trigger.GetHitVar},
	{name = "HitPauseTime", func = trigger.HitPauseTime},
}

local function setCommonENVVars(env)
	env.print = print
	env.pairs = pairs
end

local function createENV(csObjUnit)
	local warp = function(func)
		return function( ... )
		    return func(csObjUnit, ...)
		end
	end
	local env = {}
	setCommonENVVars(env)
	for _, controller in pairs(controllers) do
		env[controller.name] = warp(controller.func)
	end
	for _, trigger in pairs(triggers) do
		env[trigger.name] = warp(trigger.func)
	end
	return env
end

local function createFSM(scriptName, csObjUnit)
	local fsm = require ("Chars/" .. scriptName .. "/"  .. scriptName)
	local env = createENV(csObjUnit)
	local res = {
        update = function(stateNo)
        	if fsm == nil or fsm[stateNo] == nil then
		        return
	        end
            for i = -3, -1 do
    	        if fsm[i] and fsm[i].onUpdate then
		            fsm[i].onUpdate(env)
	            end
            end
	        if fsm[stateNo].onUpdate then
	            fsm[stateNo].onUpdate(env)
            end
        end,
        changeState = function(stateNo)
	        if fsm[stateNo].onEnter then
	            fsm[stateNo].onEnter(env)
            end
        end
    }
    return res
end

return {create = createFSM}