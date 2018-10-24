local config = {
    --Velocity
	["walk.fwd"]  = {x=9.6},
	["walk.back"] = {x=-8.8},
	["run.fwd"]  = {x=18.4, y=0},
    ["run.back"] = {x=-18, y=-15.2} ,
    ["jump.neu"] = {x=0, y=-33.6}, --Neutral jumping velocity (x, y)
    ["jump.back"] = {x=-10.2, y=-33.6},
    ["jump.fwd"] = {x=10, y=-33.6},
}

return config