local Enums = require "Chars/Enums"

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
	   PhysicsSet(Enums.PhysicsType.S)
	   CtrlSet(false)
	   MoveTypeSet(Enums.MoveType.A)
	end,
	onUpdate = function(_ENV)
        if AnimElem() == 2 then
            HitDefSet({
                id = 200,
                hitType = Enums.HitType.KnockBack,
                knockBackType = Enums.KnockBackType.Low,
                knockBackForceLevel = Enums.KnockBackForceLevel.Light,
                hitDamage = 10,
                hitPauseTime = {11, 13},
                hitSlideTime = 9,
                groundVel = {-5, 0},
                airVel = {-2, 2},

                guardDamage = 1,
                guardPauseTilme = {11, 13},
                guardSlideTime = 9,        
            })      
        end
	    if LeftAnimTime() == 0 then
			ChangeState(0)
		end
	end,
}

return M