local M = {}

M[-2] = {
    onUpdate = function(char)
    	if (char.env.stateNo == 0 or char.env.stateNo == 20) and char.env:CommandTest("x") then
    		char.ctl:ChangeState(200)
    	end
    end
}

M[200] = {
	onEnter = function(char)
	   char.ctl:ChangeAnim(200)
	   char.ctl:PhysicsSet("S")
	   char.ctl:CtrlSet(false)
	   char.ctl:MoveTypeSet("A")
	   char.isHit = false
	end,
	onExit = function(char)
	    char.isHit = nil
	end,
	onUpdate = function(char)
        if char.isHit == false and char.env.animFrame <= 4 and char.env.animFrame >= 3 then
        	if char.ctl:HitDef({
        		    attackType = "A", -- attack or throw
        		    attackPart = "Hand_L",
        		    attackLevel = 0, 
        		    hitFlag = "M",
        		    guardFlag = "M",
                    --p1StateNo = 0,
                    --p2StateNo = 0,
        		    hitDamage = 10,
        		    guardDamage = 1,
        		    hitPauseTime = {11, 13},
        		    hitSlideTime = 9,
        		    guardPauseTilme = {11, 13},
        		    guardSlideTime = 9, 
        		    groundVel = {x = 5, y = 0},
        		    airVel = {x = -10, y = 8},

        		}) then
        		char.isHit = true
        	end
        end
	    if char.env.leftAnimFrame == 0 then
			char.ctl:ChangeState(0)
		end
	end,
}

return M