local Enums = require "Chars/Enums"
local M = {}

--[[
M[] = {
	onEnter = function(_ENV)
	  --to do	
	end,
	onUpdate = function(_ENV)	
	  --to do
	end,
}
--]]

local function handleChangeFacing(_ENV)
	local facing = 1
	if P2Dist().x < 0 then
		facing = -1
	end
	if Facing() ~= facing then
	    if StateNo() == 0 and StateNo() ~= 5 then
       	   ChangeState(5)
        end
        if StateNo() == 11 and StateNo() ~= 6 then
       	   ChangeState(6)
        end
	end
end

M[-1] = {
	onUpdate = function(_ENV)
	    if (StateNo() == 0 or StateNo() == 20) and CommandTest("FF") then
			ChangeState(100)
		end
		if CommandTest("holdup") and (StateNo() == 0 or StateNo() == 20 or StateNo() == 100) then
			ChangeState(40) --jump
		end
	    if (CommandTest("holdfwd") or CommandTest("holdback")) and StateNo() == 0 then
			ChangeState(20) --walk
		end
		handleChangeFacing(_ENV)
		if (StateNo() == 0 or StateNo() == 20) and CommandTest("holddown")  then
			ChangeState(10)
		end
	end,
}

M[0] = {
    onEnter = function(_ENV)
        MoveTypeSet(Enums.MoveType.I)
        PhysicsSet(Enums.PhysicsType.S)
		CtrlSet(true)
        ChangeAnim(0)
	end,
	onUpdate = function(_ENV)
	    
	end,
}

--turn stand
M[5] = {
	onEnter = function(_ENV)
	    if AnimExist(5) then
	        ChangeAnim(5)
	    else
	    	ChangeState(0)
	    end
	end,
	
	onUpdate = function(_ENV)
	    if LeftAnimTime() <= 0 then
	    	local curFacing = Facing()
	        ChangeFacing(-curFacing)
	        ChangeState(0)
	    end
	end,
}

--turn crouch
M[6] = {
	onEnter = function(_ENV)
	    if AnimExist(6) then
	        ChangeAnim(6)
	    else
	    	ChangeState(11)
	    end
	end,
	
	onUpdate = function(_ENV)
	    if LeftAnimTime() <= 0 then
	    	local curFacing = Facing()
	        ChangeFacing(-curFacing)
	        ChangeState(11)
	    end
	end,
}

--stand to crouch
M[10] = {
	onEnter = function(_ENV)
	    PhysicsSet(Enums.PhysicsType.S)
	    if AnimExist(10) then
	        ChangeAnim(10)
	    else
	    	ChangeState(0)
	    end
	end,
	onUpdate = function(_ENV)
	    if LeftAnimTime() <= 0 then
	        ChangeState(11)
	    end
	end,
}

--crouch
M[11] = {
	onEnter = function(_ENV)
	    PhysicsSet(Enums.PhysicsType.C)
	    if AnimExist(11) then
	        ChangeAnim(11)
	    else
	    	ChangeState(0)
	    end
	end,
	onUpdate = function(_ENV)
	    if not CommandTest("holddown") then
	    	ChangeState(12)
	    end
	end,
}

--crouch to stand
M[12] = {
    onEnter = function(_ENV)
        PhysicsSet(Enums.PhysicsType.S)
	    if AnimExist(12) then
	        ChangeAnim(12)
	    else
	    	ChangeState(0)
	    end
	end,
	onUpdate = function(_ENV)
	     if LeftAnimTime() <= 0 then
	    	ChangeState(0)
	    end
	end,
}

--walk
M[20] = {
	onEnter = function(_ENV)
		CtrlSet(true)
	end,
    onUpdate = function(_ENV)
		if Anim() ~= 20 and CommandTest("holdfwd") then
			ChangeAnim(20)
		end
		if Anim() ~= 21 and CommandTest("holdback") then
			ChangeAnim(21)
		end
		if Anim() == 20 and CommandTest("holdfwd") then
			VelSet(2.5, 0)
		end
		if Anim() == 21 and CommandTest("holdback") then
			VelSet(-2.5, 0)
		end
		if not CommandTest("holdback") and not CommandTest("holdfwd") then
            ChangeState(0)
		end
	end,
}

--jump start
M[40] = {
    onEnter = function(_ENV)
    	PhysicsSet(Enums.PhysicsType.S)
    	MoveTypeSet(Enums.MoveType.I)
    	ChangeAnim(40)
    	CtrlSet(true)
    	if CommandTest("holdback") then
    		_ENV.jumpDir = -1
    	elseif CommandTest("holdfwd") then
    		_ENV.jumpDir = 1
    	else
    		_ENV.jumpDir = 0
    	end
    end,
    onUpdate = function(_ENV)
    	if LeftAnimTime() <= 0 then
    	   ChangeState(41)
    	end
    end
}

--jump up
M[41] = {
	onEnter = function(_ENV)
		PhysicsSet(Enums.PhysicsType.A)
		MoveTypeSet(Enums.MoveType.I)
		CtrlSet(true)
		ChangeAnim(41)
		VelSet(_ENV.jumpDir*2, 7)
	end,
	onUpdate = function(_ENV)
		if Vel().y <= 0 then
			ChangeState(42)
		end
	end
}

--jump down
M[42] = {
    onEnter = function(_ENV)
        ChangeAnim(42)
    end,
    onUpdate = function(_ENV)
        if JustOnGround() then
        	ChangeState(47)
        end
    end
}

--land
M[47] = {
	onEnter = function(_ENV)
		MoveTypeSet(Enums.MoveType.I)
		PhysicsSet(Enums.PhysicsType.S)
		CtrlSet(true)
		ChangeAnim(47)
		VelSet(0, 0)
	end,
	onUpdate = function(_ENV)
		if LeftAnimTime() <= 0 then
		    ChangeState(0)
		end
	end
}

--run
M[100] = {
	onEnter = function(_ENV)
		MoveTypeSet(Enums.MoveType.I)
		PhysicsSet(Enums.PhysicsType.S)
		CtrlSet(true)
		ChangeAnim(100)	
	end,
	onUpdate = function(_ENV)
		VelSet(5, 0)
		if not CommandTest("holdfwd") then
			ChangeState(101)
		end
	end,
}

--run stop
M[101] = {
	onEnter = function(_ENV)
		if AnimExist(101) then
			ChangeAnim(101)
		else
			ChangeState(0)
		end
	end,
	onUpdate = function(_ENV)
		if LeftAnimTime() <= 0 then
			ChangeState(0)
		end
	end,
}


--get-hit(shaking)
M[5000] = {
	onEnter = function(_ENV)
	    local physicsType = PhysicsType()
	    print(physicsType)
		MoveTypeSet(Enums.MoveType.H)
		PhysicsSet(Enums.PhysicsType.N)
		CtrlSet(false)
		local _nextState = -1
		local anim = 5000
		if GetHitVar("hitType") == Enums.HitType.KnockBack then
			if physicsType == Enums.PhysicsType.S then
				anim = 5000 + 10*GetHitVar("knockBackType") + GetHitVar("knockBackForceLevel")
				_nextState = 5010
		    elseif physicsType == Enums.PhysicsType.C then
		    	anim = 5020 + GetHitVar("knockBackForceLevel")
		    	_nextState = 5020
		    elseif physicsType == Enums.PhysicsType.A then
		    	anim = 5040
		    	_nextState = 5040
		    end	
		elseif GetHitVar("hitType") == Enums.HitType.KnockAway then
			anim = 5050 + GetHitVar("knockAwayType")
			_nextState = 5050
		end
		ChangeAnim(anim)
		_ENV._nextState = _nextState
		print(_nextState)
	end,
	onUpdate = function(_ENV)
	    --freeze anim
		ChangeAnim(Anim())
        if StateTime() > GetHitVar("hitShakeTime") then
        	if _ENV._nextState ~= -1 then
        		ChangeState(_ENV._nextState)
        	end
        end
	end,
}

--ground stand knock back
M[5010] = {
	onEnter = function(_ENV)
		MoveTypeSet(Enums.MoveType.H)
		PhysicsSet(Enums.PhysicsType.S)
		CtrlSet(false)
		VelSet(GetHitVar("groundVel"))
	end,
	onUpdate = function(_ENV)
		if StateTime() > GetHitVar("hitSlideTime") then
			ChangeState(0)
		end
	end,
}

--ground crouch knock back
M[5020] = {
	onEnter = function(_ENV)
		MoveTypeSet(Enums.MoveType.H)
		PhysicsSet(Enums.PhysicsType.C)
		CtrlSet(false)
		VelSet(GetHitVar("groundVel"))
		ChangeAnim(5020)
	end,
	onUpdate = function(_ENV)
		if StateTime() > GetHitVar("hitSlideTime") then
			ChangeState(11)
		end
	end,
}

--air knock back
M[5040] = {
	onEnter = function (_ENV)
		MoveTypeSet(Enums.MoveType.H)
		PhysicsSet(Enums.PhysicsType.A)
		ChangeAnim(5040)
		VelSet(GetHitVar("airVel"))
	end,
	onUpdate = function(_ENV)
	    if JustOnGround() then
	    	ChangeState(47)
	    end
	end,
}

--knock away type 1
M[5050] = {
	onEnter = function(_ENV)
	end,
	onUpdate = function(_ENV)
		
	end
}

return M