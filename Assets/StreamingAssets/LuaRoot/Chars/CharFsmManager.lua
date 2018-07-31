local trigger = require "trigger.cs"

local function createCharFsm(scriptName, csObjChar)
	local characterModule = require ("Chars/" .. scriptName .. "/"  .. scriptName)
	local charFsm = characterModule.new(csObjChar)
	return {
	    update = function()
	       charFsm:update()
        end
    }
end

return { create = createCharFsm}