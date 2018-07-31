local Char = require "Chars/Char"
local fsm = require "Chars/Origin/OriginFSM"
fsm.__loadall()

local M = {} 

function M.new(csObjChar)
	return Char.new(csObjChar, fsm)
end

return M