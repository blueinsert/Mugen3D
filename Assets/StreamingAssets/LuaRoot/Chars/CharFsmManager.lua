local trigger = require "trigger.cs"

CHAR_FSMS = {}

local function createCharFsm(slot, scriptName, csObjChar)
	local characterModule = require ("Chars/" .. scriptName .. "/"  .. scriptName)
	local charFsm = characterModule.new(csObjChar)
	charFsm.slot = slot
	CHAR_FSMS[slot] = charFsm
	return {
	    update = function()
	       charFsm:update()
        end
    }
end

return { create = createCharFsm}