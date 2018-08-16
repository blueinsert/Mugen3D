require "Chars/enums"
local M = {}

--[[
M[] = {
	onEnter = function(env, ctl, cfg)
		
	end,

	onExit = function(env, ctl, cfg)
		
	end,
	
	onUpdate = function(env, ctl, cfg)
		
	end,
}
--]]

M[-1] = {
    onEnter = function(char)
	end,
	
	onExit = function(char)
	end,
	
	onUpdate = function(char)
	    if (char.env:CommandTest("holdfwd") or char.env:CommandTest("holdback")) and char.env:StateNo() == 0 then
			char.ctl:ChangeState(20) --walk
			return
		end
		if char.env:CommandTest("holdup") and (char.env:StateNo() == 0 or char.env:StateNo() == 20 or char.env:StateNo() == 100) then
			char.ctl:ChangeState(40) --jump
			return
		end
	end,
}

M[0] = {
    onEnter = function(char)
	end,
	
	onExit = function(char)
	end,
	
	onUpdate = function(char)
	    if char.env:Anim() ~= 0 then
	   	   char.ctl:ChangeAnim(0)
	    end
	end,
}

--walk
M[20] = {
	onEnter = function(char)
		char.ctl:CtrlSet(true)
	end,

	onExit = function(char)
		
	end,

	onUpdate = function(char)
		if char.env:Anim() ~= 20 and char.env:CommandTest("holdfwd") then
			char.ctl:ChangeAnim(20)
		end
		if char.env:Anim() ~= 21 and char.env:CommandTest("holdback") then
			char.ctl:ChangeAnim(21)
		end
		if char.env:Anim() == 20 and char.env:CommandTest("holdfwd") then
			char.ctl:VelSet(2.5, 0)
		end
		if char.env:Anim() == 21 and char.env:CommandTest("holdback") then
			char.ctl:VelSet(-2.5, 0)
		end
		if not char.env:CommandTest("holdback") and not char.env:CommandTest("holdfwd") then
            char.ctl:ChangeState(0)
		end
	end,
}

--jump start
M[40] = {
    onEnter = function(char)
        --print("jumpStart")
    	char.ctl:PhysicsSet(PhysicsType.S)
    	char.ctl:MoveTypeSet(MoveType.I)
    	char.ctl:ChangeAnim(40)
    	char.ctl:CtrlSet(true)
    	if char.env:CommandTest("holdback") then
    		char.jumpDir = -1
    	elseif char.env:CommandTest("holdfwd") then
    		char.jumpDir = 1
    	else
    		char.jumpDir = 0
    	end
    end,
    onExit = function(char)
    	
    end,
    onUpdate = function(char)
    	if char.env:LeftAnimTime() <= 0 then
    		char.ctl:ChangeState(41)
    	end
    end
}

--jump up
M[41] = {
	onEnter = function(char)
		char.ctl:PhysicsSet(PhysicsType.A)
		char.ctl:MoveTypeSet(MoveType.I)
		char.ctl:CtrlSet(true)
		char.ctl:ChangeAnim(41)
		char.ctl:VelSet(char.jumpDir*2, 7)
	end,
	onExit = function(char)
		
	end,
	onUpdate = function(char)
		if char.env:Vel().y <= 0 then
			char.ctl:ChangeState(42)
		end
	end
}
--jump down
M[42] = {
    onEnter = function(char)
        char.ctl:ChangeAnim(42, "Once")
    end,
    onUpdate = function(char)
        if char.env:JustOnGround() then
        	char.ctl:ChangeState(47)
        end
    	-- body
    end
}

--land
M[47] = {
	onEnter = function(char)
		char.ctl:MoveTypeSet(MoveType.I)
		char.ctl:PhysicsSet(PhysicsType.S)
		char.ctl:CtrlSet(true)
		char.ctl:ChangeAnim(47)
		char.ctl:VelSet(0, 0)
	end,
	onUpdate = function(char)
		if char.env:LeftAnimTime() <= 0 then
			char.ctl:ChangeState(0)
		end
	end
}

--[[
M[-1] = {
	onUpdate = function(char)
	    if char.env:CommandTest("FF") and (char.env.stateNo == 0 or char.env.stateNo == 20) then
        	char.ctl:ChangeState(100) --run
        	return
        end
        if char.env:CommandTest("BB") and (char.env.stateNo == 0 or char.env.stateNo == 20) then
        	char.ctl:ChangeState(105) --run
        	return
        end
		if (char.env:CommandTest("holdfwd") or char.env:CommandTest("holdback")) and char.env.stateNo == 0 then
			char.ctl:ChangeState(20) --walk
			return
		end
		if char.env:CommandTest("holdup") and (char.env.stateNo == 0 or char.env.stateNo == 20 or char.env.stateNo == 100) then
			char.ctl:ChangeState(40) --jump
			return
		end
	end,
}

--stand
M[0] = {
    onEnter = function(char)
        char.ctl:CtrlSet(true)
	end,
	
	onExit = function(char)
	end,
	
	onUpdate = function(char)
	   if char.env.animNo ~=0 then
	   	   char.ctl:ChangeAnim(0)
	   end
	end,
}

--walk
M[20] = {
	onEnter = function(char)
		char.ctl:CtrlSet(true)
	end,

	onExit = function(char)
		
	end,

	onUpdate = function(char)
		if char.env.animNo ~= 20 and char.env:CommandTest("holdfwd") then
			char.ctl:ChangeAnim(20)
		end
		if char.env.animNo ~= 21 and char.env:CommandTest("holdback") then
			char.ctl:ChangeAnim(21)
		end
		if char.env.animNo == 20 and char.env:CommandTest("holdfwd") then
			char.ctl:VelSet(3, 0)
		end
		if char.env.animNo == 21 and char.env:CommandTest("holdback") then
			char.ctl:VelSet(-3, 0)
		end
		if not char.env:CommandTest("holdback") and not char.env:CommandTest("holdfwd") then
            char.ctl:ChangeState(0)
		end
	end,
}

--jump start
M[40] = {
    onEnter = function(char)
        --print("jumpStart")
    	char.ctl:PhysicsSet("S")
    	char.ctl:MoveTypeSet("I")
    	char.ctl:ChangeAnim(40)
    	char.ctl:CtrlSet(true)
    	if char.env:CommandTest("holdback") then
    		char.jumpDir = -1
    	elseif char.env:CommandTest("holdfwd") then
    		char.jumpDir = 1
    	else
    		char.jumpDir = 0
    	end
    end,
    onExit = function(char)
    	
    end,
    onUpdate = function(char)
    	if char.env.leftAnimFrame == 0 then
    		char.ctl:ChangeState(50)
    	end
    end
}

--jump up
M[50] = {
	onEnter = function(char)
		char.ctl:PhysicsSet("A")
		char.ctl:MoveTypeSet("I")
		char.ctl:CtrlSet(true)
		char.ctl:ChangeAnim(41, "Once")
		char.ctl:VelSet(char.jumpDir*2, 7)
	end,
	onExit = function(char)
		
	end,
	onUpdate = function(char)
		if char.env.vel.y <=0 then
			char.ctl:ChangeState(51)
		end
	end
}
--jump down
M[51] = {
    onEnter = function(char)
        char.ctl:ChangeAnim(42, "Once")
    end,
    onUpdate = function(char)
        if char.env.justOnGround then
        	char.ctl:ChangeState(52)
        end
    	-- body
    end
}

--land
M[52] = {
	onEnter = function(char)
		char.ctl:MoveTypeSet("I")
		char.ctl:PhysicsSet("S")
		char.ctl:CtrlSet(true)
		char.ctl:ChangeAnim(47)
		char.ctl:VelSet(0, 0)
	end,
	onUpdate = function(char)
		if char.env.leftAnimFrame == 0 then
			char.ctl:ChangeState(0)
		end
	end
}

--run
M[100] = {
	onEnter = function(char)
		char.ctl:MoveTypeSet("I")
		char.ctl:PhysicsSet("S")
		char.ctl:CtrlSet(true)
		char.ctl:ChangeAnim(100)	
	end,
	onUpdate = function(char)
		char.ctl:VelSet(5, 0)
		if not char.env:CommandTest("holdfwd") then
			char.ctl:ChangeState(0)
		end
	end,
}

--hope back
M[105] = {
    onEnter = function(char)
    	char.ctl:MoveTypeSet("I")
    	char.ctl:CtrlSet(true)
    	char.ctl:PhysicsSet("A")
    	char.ctl:ChangeAnim(105, "Once")
    	char.ctl:VelSet(-4, 2)
    end,
    onUpdate = function(char)
    	if char.env.justOnGround then
    		char.ctl:ChangeState(106)
    	end
    end,
}

--hope back land
M[106] = {
	onEnter = function(char)
		char.ctl:MoveTypeSet("I")
		char.ctl:PhysicsSet("S")
		char.ctl:CtrlSet(true)
		char.ctl:ChangeAnim(106)
		char.ctl:VelSet(0,0)
	end,

	onUpdate = function(char)		
	    if char.env.leftAnimFrame == 0 then
			char.ctl:ChangeState(0)
		end
	end
}

--stand get-hit(shaking)
M[5000] = {
	onEnter = function(char)
	    print("5000 onEnter")
		char.ctl:MoveTypeSet("I")
		char.ctl:PhysicsSet("N")
		char.ctl:CtrlSet(false)
		print("ChangeAnim:" .. (5000 + char.hitDefData.attackLevel))
		char.ctl:ChangeAnim(5000 + char.hitDefData.attackLevel, "Once")
	end,
	onExit = function(char)
		
	end,
	onUpdate = function(char)
	    print("5000 onUpdate")
	    --freeze anim
		char.ctl:ChangeAnim(5000 + char.hitDefData.attackLevel, "Once")
        if char.env.stateTime > char.hitDefData.hitPauseTime[2] then
        	char.ctl:ChangeState(5001)
        end
	end,
}

M[5001] = {
	onEnter = function(char)
		char.ctl:MoveTypeSet("I")
		char.ctl:PhysicsSet("S")
		char.ctl:CtrlSet(false)
		char.ctl:VelSet(char.hitDefData.groundVel.x, char.hitDefData.groundVel.y)
	end,
	onExit = function(char)
		
	end,
	onUpdate = function(char)
		if char.env.leftAnimFrame == 0 then
			char.ctl:ChangeAnim(char.env.animNo + 5)
		end
		if char.env.stateTime > char.hitDefData.hitSlideTime then
			char.ctl:ChangeState(0)
		end
	end,
}
--]]
return M