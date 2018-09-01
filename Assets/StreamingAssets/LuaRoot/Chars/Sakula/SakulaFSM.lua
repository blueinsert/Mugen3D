local FSMS = {
    "Chars/CharFSMCommon",
    "Chars/Sakula/SakulaFSM_N",
    "Chars/Sakula/SakulaFSM_H",
}

local M = {}

function M.__loadall()
    for _, name in pairs(FSMS) do
        local t = require(name)
        for k, v in pairs(t) do
            M[k] = v
        end
    end
end

return M

