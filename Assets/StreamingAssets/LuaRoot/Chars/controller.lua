local M = {}

function M.new(char)
    local obj = {}
	obj.char = char
	setmetatable(obj, {__index = M})
	return obj
end

function M:ChangeState(stateNo)
    self.char:changeState(stateNo)
end

function M:ChangeAnim(animNo)
    self.char.player.animCtr:ChangeAnim(animNo)
end

function M:PhysicsSet(physics)
    local physicsType
    if physics == "S" then
        physicsType = CS.Mugen3D.Core.PhysicsType.S
    elseif physics == "C" then
        physicsType = CS.Mugen3D.Core.PhysicsType.C
    elseif physics == "A" then
        physicsType = CS.Mugen3D.Core.PhysicsType.A
    else
        physicsType = CS.Mugen3D.Core.PhysicsType.N
    end
    self.char.player.status.physicsType = physicsType
end

function M:MoveTypeSet(moveTypeStr) 
    local moveType
     if moveTypeStr == "S" then
        moveType = CS.Mugen3D.Core.PhysicsType.S
    elseif moveTypeStr == "C" then
        moveType = CS.Mugen3D.Core.PhysicsType.C
    elseif moveTypeStr == "A" then
        moveType = CS.Mugen3D.Core.PhysicsType.A
    else
        moveType = CS.Mugen3D.Core.PhysicsType.N
    end
    self.char.player.status.moveType = moveType
end

function M:VelSet(x, y)
    self.char.player.moveCtr:VelSet(toNumber(x), toNumber(y))
end 

function M:CtrlSet(canCtrl)
    self.char.player.status.ctrl = canCtrl
end

function M:Pause(duration)
    self.char.player:Pause(duration)
end

--[[
function M:MoveTypeSet(moveType)
    self.ctl:MoveTypeSet(self.char.player, moveType)
end

function M:CtrlSet(canCtrl)
    self.ctl:CtrlSet(self.char.player, canCtrl)
end

function M:CreateHelper(data)
    self.ctl:CreateHelper(self.char.player, data)
end

local AttackLevel = {
    light = 1,
    medium = 2,
    hard = 3,
}

local function onHit(attacker, target, hitDefData)
    print("attack")
    print(attacker)
    print("target")
    print(target)
    target.hitDefData = hitDefData
    if target.env.moveType == "D" then
        attacker.ctl:Pause(hitDefData.guardPauseTime[1])
        target.ctl:ChangeState(150)
    else
        attacker.ctl:Pause(hitDefData.hitPauseTime[1])
        target.ctl:ChangeState(5000)
    end
end

local function onGrab(attacker, target, hitDefData)
    -- body
end

function M:HitDef(hitDefData)
    local isHit, target = self.char.player:IsHitOthers(CS.Mugen3D.HitPart.Hand_L)
    if not isHit then
        return
    end
    local tarChar = target.fsm
    if hitDefData.attackType == "T" then
        onGrab(self.char, tarChar, hitDefData)
    else
        onHit(self.char, tarChar, hitDefData)
    end
    return isHit
end

function M:LifeAdd(lifeAdd)
    self.ctl:Life(self.char.player, lifeAdd)
end

function M:LifeSet(life)
    self.ctl:LifeSet(self.char.player, life)
end

function M:VelSet(x, y)
    self.ctl:VelSet(self.char.player, x, y)
end 

function M:Pause(duration)
    self.ctl:Pause(self.char.player, duration)
end
--]]

return M