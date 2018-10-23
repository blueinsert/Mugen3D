local Enums = require "Chars/Enums"

local M = {}

M[-2] = {
    onUpdate = function(_ENV)
        if (StateNo() == 0 or StateNo() == 20) and CommandTest("x") and math.abs(P2Dist().x) > 0.8 then
            ChangeState(200)
        end
        if (StateNo() == 0 or StateNo() == 20) and CommandTest("x") and math.abs(P2Dist().x) <= 0.8 then
            ChangeState(205)
        end
        if (StateNo() == 0 or StateNo() == 20) and CommandTest("a") then
            ChangeState(220)
        end
        if (StateNo() == 0 or StateNo() == 20) and CommandTest("y") and math.abs(P2Dist().x) > 0.8 then
            ChangeState(240)
        end
         if (StateNo() == 0 or StateNo() == 20) and CommandTest("y") and math.abs(P2Dist().x) <= 0.8 then
            ChangeState(245)
        end
         if (StateNo() == 0 or StateNo() == 20) and CommandTest("b") and math.abs(P2Dist().x) > 0.8 then
            ChangeState(260)
        end
        if (StateNo() == 0 or StateNo() == 20) and CommandTest("b") and math.abs(P2Dist().x) <= 0.8 then
            ChangeState(265)
        end
        if (StateNo() == 11) and CommandTest("x") then
            ChangeState(400)
        end
        if (StateNo() == 11) and CommandTest("a") then
            ChangeState(420)
        end
        if (StateNo() == 11) and CommandTest("y") then
            ChangeState(440)
        end
        if (StateNo() == 11) and CommandTest("b") then
            ChangeState(460)
        end
        if PhysicsType() == Enums.PhysicsType.A and Ctrl() and CommandTest("x") then
            ChangeState(600)
        end
        if PhysicsType() == Enums.PhysicsType.A and Ctrl() and CommandTest("a") then
            ChangeState(620)
        end
        if PhysicsType() == Enums.PhysicsType.A and Ctrl() and CommandTest("y") then
            ChangeState(640)
        end
        if PhysicsType() == Enums.PhysicsType.A and Ctrl() and CommandTest("b") then
            ChangeState(660)
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

M[205] = {
    onEnter = function(_ENV)
       ChangeAnim(205)
       PhysicsSet(Enums.PhysicsType.S)
       CtrlSet(false)
       MoveTypeSet(Enums.MoveType.A)
    end,
    onUpdate = function(_ENV)
        if AnimElem() == 1 then
            HitDefSet({
                id = 205,
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

M[220] = {
    onEnter = function(_ENV)
       ChangeAnim(220)
       PhysicsSet(Enums.PhysicsType.S)
       CtrlSet(false)
       MoveTypeSet(Enums.MoveType.A)
    end,
    onUpdate = function(_ENV)
        if AnimElem() == 1 then
            HitDefSet({
                id = 220,
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

M[240] = {
    onEnter = function(_ENV)
       ChangeAnim(240)
       PhysicsSet(Enums.PhysicsType.S)
       CtrlSet(false)
       MoveTypeSet(Enums.MoveType.A)
    end,
    onUpdate = function(_ENV)
        if AnimElem() == 3 then
            HitDefSet({
                id = 240,
                hitType = Enums.HitType.KnockBack,
                knockBackType = Enums.KnockBackType.Low,
                knockBackForceLevel = Enums.KnockBackForceLevel.Light,
                hitDamage = 10,
                hitPauseTime = {11, 13},
                hitSlideTime = 16,
                groundVel = {-7, 0},
                airVel = {-2, 2},

                guardDamage = 1,
                guardPauseTilme = {11, 13},
                guardSlideTime = 16,        
            })     
        end 
        if LeftAnimTime() == 0 then
            ChangeState(0)
        end
    end,
}

M[245] = {
    onEnter = function(_ENV)
       ChangeAnim(245)
       PhysicsSet(Enums.PhysicsType.S)
       CtrlSet(false)
       MoveTypeSet(Enums.MoveType.A)
    end,
    onUpdate = function(_ENV)
        if AnimElem() == 3 then
            HitDefSet({
                id = 245,
                hitType = Enums.HitType.KnockBack,
                knockBackType = Enums.KnockBackType.Low,
                knockBackForceLevel = Enums.KnockBackForceLevel.Light,
                hitDamage = 10,
                hitPauseTime = {11, 13},
                hitSlideTime = 16,
                groundVel = {-7, 0},
                airVel = {-2, 2},

                guardDamage = 1,
                guardPauseTilme = {11, 13},
                guardSlideTime = 16,        
            })     
        end 
        if LeftAnimTime() == 0 then
            ChangeState(0)
        end
    end,
}

M[260] = {
    onEnter = function(_ENV)
       ChangeAnim(260)
       PhysicsSet(Enums.PhysicsType.S)
       CtrlSet(false)
       MoveTypeSet(Enums.MoveType.A)
    end,
    onUpdate = function(_ENV)
        if AnimElem() == 3 then
            HitDefSet({
                id = 260,
                hitType = Enums.HitType.KnockBack,
                knockBackType = Enums.KnockBackType.Low,
                knockBackForceLevel = Enums.KnockBackForceLevel.Light,
                hitDamage = 10,
                hitPauseTime = {11, 13},
                hitSlideTime = 16,
                groundVel = {-7, 0},
                airVel = {-2, 2},

                guardDamage = 1,
                guardPauseTilme = {11, 13},
                guardSlideTime = 16,        
            })     
        end 
        if LeftAnimTime() == 0 then
            ChangeState(0)
        end
    end,
}

M[265] = {
    onEnter = function(_ENV)
       ChangeAnim(265)
       PhysicsSet(Enums.PhysicsType.S)
       CtrlSet(false)
       MoveTypeSet(Enums.MoveType.A)
    end,
    onUpdate = function(_ENV)
        if AnimElem() == 4 then
            HitDefSet({
                id = 265,
                hitType = Enums.HitType.KnockBack,
                knockBackType = Enums.KnockBackType.Low,
                knockBackForceLevel = Enums.KnockBackForceLevel.Light,
                hitDamage = 10,
                hitPauseTime = {11, 13},
                hitSlideTime = 16,
                groundVel = {-7, 0},
                airVel = {-2, 2},

                guardDamage = 1,
                guardPauseTilme = {11, 13},
                guardSlideTime = 16,        
            })     
        end 
        if LeftAnimTime() == 0 then
            ChangeState(0)
        end
    end,
}

M[400] = {
    onEnter = function(_ENV)
       ChangeAnim(400)
       PhysicsSet(Enums.PhysicsType.C)
       CtrlSet(false)
       MoveTypeSet(Enums.MoveType.A)
    end,
    onUpdate = function(_ENV)
        if AnimElem() == 1 then
            HitDefSet({
                id = 400,
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
            ChangeState(11)
        end
    end,
}

M[420] = {
    onEnter = function(_ENV)
       ChangeAnim(420)
       PhysicsSet(Enums.PhysicsType.C)
       CtrlSet(false)
       MoveTypeSet(Enums.MoveType.A)
    end,
    onUpdate = function(_ENV)
        if AnimElem() == 1 then
            HitDefSet({
                id = 420,
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
            ChangeState(11)
        end
    end,
}

M[440] = {
    onEnter = function(_ENV)
       ChangeAnim(440)
       PhysicsSet(Enums.PhysicsType.C)
       CtrlSet(false)
       MoveTypeSet(Enums.MoveType.A)
    end,
    onUpdate = function(_ENV)
        if AnimElem() == 2 then
            HitDefSet({
                id = 440,
                hitType = Enums.HitType.KnockBack,
                knockBackType = Enums.KnockBackType.Low,
                knockBackForceLevel = Enums.KnockBackForceLevel.Light,
                hitDamage = 10,
                hitPauseTime = {11, 13},
                hitSlideTime = 16,
                groundVel = {-7, 0},
                airVel = {-2, 2},

                guardDamage = 1,
                guardPauseTilme = {11, 13},
                guardSlideTime = 16,        
            })     
        end 
        if LeftAnimTime() == 0 then
            ChangeState(11)
        end
    end,
}

M[460] = {
    onEnter = function(_ENV)
       ChangeAnim(460)
       PhysicsSet(Enums.PhysicsType.C)
       CtrlSet(false)
       MoveTypeSet(Enums.MoveType.A)
    end,
    onUpdate = function(_ENV)
        if AnimElem() == 1 then
            HitDefSet({
                id = 460,
                hitType = Enums.HitType.KnockAway,
                knockAwayType = Enums.KnockAwayType.Type1,
                hitDamage = 10,
                hitPauseTime = {11, 13},
                hitSlideTime = 9,
                groundVel = {-2, 2},
                airVel = {-2, 3},

                guardDamage = 1,
                guardPauseTilme = {11, 13},
                guardSlideTime = 9,        
            })     
        end 
        if LeftAnimTime() == 0 then
            ChangeState(11)
        end
    end,
}

M[600] = {
    onEnter = function(_ENV)
       ChangeAnim(600)
       PhysicsSet(Enums.PhysicsType.A)
       CtrlSet(false)
       MoveTypeSet(Enums.MoveType.A)
    end,
    onUpdate = function(_ENV)
        if AnimElem() == 1 then
            HitDefSet({
                id = 600,
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
        if JustOnGround() then
            ChangeState(47)
        end
    end,
}

M[620] = {
    onEnter = function(_ENV)
       ChangeAnim(620)
       PhysicsSet(Enums.PhysicsType.A)
       CtrlSet(false)
       MoveTypeSet(Enums.MoveType.A)
    end,
    onUpdate = function(_ENV)
        if AnimElem() == 1 then
            HitDefSet({
                id = 620,
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
        if JustOnGround() then
            ChangeState(47)
        end
    end,
}

M[640] = {
    onEnter = function(_ENV)
       ChangeAnim(640)
       PhysicsSet(Enums.PhysicsType.A)
       CtrlSet(false)
       MoveTypeSet(Enums.MoveType.A)
    end,
    onUpdate = function(_ENV)
        if AnimElem() == 2 then
            HitDefSet({
                id = 640,
                hitType = Enums.HitType.KnockBack,
                knockBackType = Enums.KnockBackType.Low,
                knockBackForceLevel = Enums.KnockBackForceLevel.Light,
                hitDamage = 10,
                hitPauseTime = {11, 13},
                hitSlideTime = 16,
                groundVel = {-7, 0},
                airVel = {-2, 2},

                guardDamage = 1,
                guardPauseTilme = {11, 13},
                guardSlideTime = 16,
            })     
        end 
        if JustOnGround() then
            ChangeState(47)
        end
    end,
}

M[660] = {
    onEnter = function(_ENV)
       ChangeAnim(660)
       PhysicsSet(Enums.PhysicsType.A)
       CtrlSet(false)
       MoveTypeSet(Enums.MoveType.A)
    end,
    onUpdate = function(_ENV)
        if AnimElem() == 4 then
            HitDefSet({
                id = 660,
                hitType = Enums.HitType.KnockBack,
                knockBackType = Enums.KnockBackType.Low,
                knockBackForceLevel = Enums.KnockBackForceLevel.Light,
                hitDamage = 10,
                hitPauseTime = {11, 13},
                hitSlideTime = 16,
                groundVel = {-7, 0},
                airVel = {-2, 2},

                guardDamage = 1,
                guardPauseTilme = {11, 13},
                guardSlideTime = 16,
            })     
        end 
        if JustOnGround() then
            ChangeState(47)
        end
    end,
}

return M