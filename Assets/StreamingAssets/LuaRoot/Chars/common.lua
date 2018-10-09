local Enums = require "Chars/enums"
local M = {}

--[[
M[] = {
	onEnter = function(_ENV)
		
	end,
	onUpdate = function(_ENV)
		
	end,
}
--]]

local function handleChangeFacing(char)
	--print("handleChangeFacing")
	local enemy = char:getEnemy()
	if not enemy then
		print("enemy is null")
		return
	end
	local enemyPos = enemy.env:Pos()
	local myPos = char.env:Pos()
	local facing = 1
	if enemyPos.x < myPos.x then
		facing = -1
	end
	if char.env:Facing() ~= facing then
	    if char.env:StateNo() == 0 and char.env:StateNo() ~= 5 then
       	   char.ctl:ChangeState(5)
        end
        if char.env:StateNo() == 11 and char.env:StateNo() ~= 6 then
       	   char.ctl:ChangeState(6)
        end
	end
end

M[-1] = {
	onUpdate = function(_ENV)
	    if (StateNo() == 0 or StateNo() == 20) and CommandTest("FF") then
			ChangeState(100)
			return
		end
		if CommandTest("holdup") and (StateNo() == 0 or StateNo() == 20 or StateNo() == 100) then
			ChangeState(40) --jump
			return
		end
	    if (CommandTest("holdfwd") or CommandTest("holdback")) and StateNo() == 0 then
			ChangeState(20) --walk
			return
		end
		--handleChangeFacing(char)
		if (StateNo() == 0 or StateNo() == 20) and CommandTest("holddown")  then
			ChangeState(10)
			return
		end
	end,
}

M[0] = {
    onEnter = function(_ENV)
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

--turn stand
M[6] = {
	onEnter = function(_ENV)
	    if AnimExist(6) then
	        ChangeAnim(6)
	    else
	    	ChangeState(0)
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


--stand get-hit(shaking)
M[5000] = {
	onEnter = function(_ENV)
		MoveTypeSet(Enums.MoveType.I)
		PhysicsSet(Enums.PhysicsType.S)
		CtrlSet(false)
		ChangeAnim(5000)
	end,
	onUpdate = function(_ENV)
	    --freeze anim
		ChangeAnim(5000)
        if StateTime() > char.hitDefData.hitPauseTime[2] then
        	ChangeState(5001)
        end
	end,
}

M[5001] = {
	onEnter = function(_ENV)
		MoveTypeSet(Enums.MoveType.I)
		PhysicsSet(Enums.PhysicsType.S)
		CtrlSet(false)
		VelSet(char.hitDefData.groundVel.x, char.hitDefData.groundVel.y)
	end,
	onUpdate = function(_ENV)
		if StateTime() > hitSlideTime then
			ChangeState(0)
		end
	end,
}

return M