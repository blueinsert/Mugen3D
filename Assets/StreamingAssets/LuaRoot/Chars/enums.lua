local MoveType = {
	["A"] = 1,
	["I"] = 2,
	["D"] = 3,
	["H"] = 4, 
}

local PhysicsType = {
	["N"] = -1,
	["S"] = 1,
	["C"] = 2,
	["A"] = 3,
}

local HitType = {
	["KnockBack"] = 1,
	["KnockAway"] = 2,
	["Throw"] = 3,
}

local KnockAwayType = {
    ["Type1"] = 0,
	["Trip"] = 1,
}

local KnockBackType = {
	["High"] = 0,
	["Low"] = 1,
}

local KnockBackForceLevel = {
	["Light"] = 0,
	["Medium"] = 1,
	["Heavy"] = 2,
}

return {
    MoveType = MoveType, 
    PhysicsType = PhysicsType,
    HitType = HitType,
    KnockBackType = KnockBackType,
    KnockBackForceLevel = KnockBackForceLevel,
    KnockAwayType = KnockAwayType,
}