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
    onEnter = function(char)
	end,
	
	onExit = function(char)
	end,
	
	onUpdate = function(char)
	    if (char.env:StateNo() == 0 or char.env:StateNo() == 20) and char.env:CommandTest("FF") then
			char.ctl:ChangeState(100)
			return
		end
	    if (char.env:CommandTest("holdfwd") or char.env:CommandTest("holdback")) and char.env:StateNo() == 0 then
			char.ctl:ChangeState(20) --walk
			return
		end
		if char.env:CommandTest("holdup") and (char.env:StateNo() == 0 or char.env:StateNo() == 20 or char.env:StateNo() == 100) then
			char.ctl:ChangeState(40) --jump
			return
		end
		handleChangeFacing(char)
		if (char.env:StateNo() == 0 or char.env:StateNo() == 20) and char.env:CommandTest("holddown")  then
			char.ctl:ChangeState(10)
			return
		end
	end,
}

M[0] = {
    onEnter = function(char)
        char.ctl:ChangeAnim(0)
	end,
	
	onExit = function(char)
	end,
	
	onUpdate = function(char)
	    
	end,
}

--turn stand
M[5] = {
	onEnter = function(char)
	    if char.env:AnimExist(5) then
	        char.ctl:ChangeAnim(5)
	    else
	    	char.ctl:ChangeState(0)
	    end
	end,
	
	onExit = function(char)
	    local curFacing = char.env:Facing()
	    char.ctl:ChangeFacing(-curFacing)
	end,
	
	onUpdate = function(char)
	    if char.env:LeftAnimTime() <= 0 then
	        char.ctl:ChangeState(0)
	    end
	end,
}

--turn stand
M[6] = {
	onEnter = function(char)
	    if char.env:AnimExist(6) then
	        char.ctl:ChangeAnim(6)
	    else
	    	char.ctl:ChangeState(0)
	    end
	end,
	
	onExit = function(char)
	    local curFacing = char.env:Facing()
	    char.ctl:ChangeFacing(-curFacing)
	end,
	
	onUpdate = function(char)
	    if char.env:LeftAnimTime() <= 0 then
	        char.ctl:ChangeState(11)
	    end
	end,
}

--stand to crouch
M[10] = {
	onEnter = function(char)
	    if char.env:AnimExist(10) then
	        char.ctl:ChangeAnim(10)
	    else
	    	char.ctl:ChangeState(0)
	    end
	end,
	
	onExit = function(char)
	   
	end,
	
	onUpdate = function(char)
	    if char.env:LeftAnimTime() <= 0 then
	        char.ctl:ChangeState(11)
	    end
	end,
}

--crouch
M[11] = {
	onEnter = function(char)
	    if char.env:AnimExist(11) then
	        char.ctl:ChangeAnim(11)
	    else
	    	char.ctl:ChangeState(0)
	    end
	end,
	
	onExit = function(char)
	   
	end,
	
	onUpdate = function(char)
	    if not char.env:CommandTest("holddown") then
	    	char.ctl:ChangeState(12)
	    end
	end,
}

--crouch to stand
M[12] = {
    onEnter = function(char)
	    if char.env:AnimExist(12) then
	        char.ctl:ChangeAnim(12)
	    else
	    	char.ctl:ChangeState(0)
	    end
	end,
	
	onExit = function(char)
	   
	end,
	
	onUpdate = function(char)
	     if char.env:LeftAnimTime() <= 0 then
	    	char.ctl:ChangeState(0)
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
    onExit = function(char)
		
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
	onExit = function(char)
		
	end,
	onUpdate = function(char)
		if char.env:LeftAnimTime() <= 0 then
			char.ctl:ChangeState(0)
		end
	end
}

--run
M[100] = {
	onEnter = function(char)
		char.ctl:MoveTypeSet(MoveType.I)
		char.ctl:PhysicsSet(PhysicsType.S)
		char.ctl:CtrlSet(true)
		char.ctl:ChangeAnim(100)	
	end,
	onExit = function(char)
		
	end,
	onUpdate = function(char)
		char.ctl:VelSet(5, 0)
		if not char.env:CommandTest("holdfwd") then
			char.ctl:ChangeState(101)
		end
	end,
}

--run stop
M[101] = {
	onEnter = function(char)
	    --char.ctl:VelSet(0, 0)
		if char.env:AnimExist(101) then
			char.ctl:ChangeAnim(101)
		else
			char.ctl:ChangeState(0)
		end
	end,
	onExit = function(char)
		
	end,
	onUpdate = function(char)
		if char.env:LeftAnimTime() <= 0 then
			char.ctl:ChangeState(0)
		end
	end,
}


--stand get-hit(shaking)
M[5000] = {
	onEnter = function(char)
		char.ctl:MoveTypeSet(MoveType.I)
		char.ctl:PhysicsSet(PhysicsType.S)
		char.ctl:CtrlSet(false)
		char.ctl:ChangeAnim(5000)
	end,
	onExit = function(char)
		
	end,
	onUpdate = function(char)
	    --freeze anim
		char.ctl:ChangeAnim(5000)
        if char.env:StateTime() > char.hitDefData.hitPauseTime[2] then
        	char.ctl:ChangeState(5001)
        end
	end,
}

M[5001] = {
	onEnter = function(char)
		char.ctl:MoveTypeSet(MoveType.I)
		char.ctl:PhysicsSet(PhysicsType.S)
		char.ctl:CtrlSet(false)
		char.ctl:VelSet(char.hitDefData.groundVel.x, char.hitDefData.groundVel.y)
	end,
	onExit = function(char)
		
	end,
	onUpdate = function(char)
		if char.env:StateTime() > char.hitDefData.hitSlideTime then
			char.ctl:ChangeState(0)
		end
	end,
}
--]]
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