local Char = require "Chars/Char"
local fsm = require "Chars/Origin/OriginFSM"
fsm.__loadall()

local M = {} 

function M.new(player)
	return Char.new(player, fsm)
end

return M