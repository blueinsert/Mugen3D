local M = {}

M[-2] = {
    onUpdate = function(_ENV)
    	if (StateNo() == 0 or StateNo() == 20) and CommandTest("x") then
    		ChangeState(200)
    	end
    end
}

M[200] = {
	onEnter = function(_ENV)
	   ChangeAnim(200)
	   PhysicsSet(PhysicsType.S)
	   CtrlSet(false)
	   MoveTypeSet(MoveType.A)
       _ENV.hasHit = false
	end,
	onUpdate = function(_ENV)
        if AnimElem() == 2 and not _ENV.hasHit then
            HitDefSet({
                hitDamage = 10,
                guardDamage = 1,
                hitPauseTime = {11, 13},
                hitSlideTime = 9,
                guardPauseTilme = {11, 13},
                guardSlideTime = 9, 
                groundVel = {-5, 0},
                airVel = {-10, 8},
            })      
        end
	    if LeftAnimTime() == 0 then
			ChangeState(0)
		end
	end,
}

return M