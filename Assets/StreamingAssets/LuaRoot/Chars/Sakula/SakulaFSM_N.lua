local M = {}

M[-2] = {
    onUpdate = function(char)
    	if (char.env:StateNo() == 0 or char.env:StateNo() == 20) and char.env:CommandTest("x") then
    		char.ctl:ChangeState(200)
    	end
    end
}

M[200] = {
	onEnter = function(char)
	   char.ctl:ChangeAnim(200)
	   char.ctl:PhysicsSet(PhysicsType.S)
	   char.ctl:CtrlSet(false)
	   char.ctl:MoveTypeSet(MoveType.A)
       char.hasHit = false
	end,
	onExit = function(char)
	end,
	onUpdate = function(char)
        if char.env:AnimElem() == 2 and not char.hasHit then
            char.ctl:HitDefSet({
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
                groundVel = {x = -5, y = 0},
                airVel = {x = -10, y = 8},
            })      
        end
	    if char.env:LeftAnimTime() == 0 then
			char.ctl:ChangeState(0)
		end
	end,
}

return M