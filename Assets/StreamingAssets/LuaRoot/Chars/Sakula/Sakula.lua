local FSMS = {
    "Chars/Common",
    "Chars/Sakula/Sakula_N",
    "Chars/Sakula/Sakula_H",
}

local M = {}

function __loadall()
    for _, name in pairs(FSMS) do
        local t = require(name)
        for k, v in pairs(t) do
            M[k] = v
        end
    end
end

__loadall()

return M

