--[[
local oldPrint = print
print = function (...)
    local trace = debug.traceback("", 3)
    oldPrint(... , " (at " .. debug.getinfo(2).short_src .. ":" .. debug.getinfo(2).currentline .. ")" .. trace)
end
print("Prelude")
--]]

