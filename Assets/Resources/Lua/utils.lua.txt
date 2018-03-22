-- Copyright (C) 2016 Joywinds Inc.

local pairs = pairs
local type = type
local strfmt = string.format
local osdate = os.date
local ostime = os.time

local utils = {}

function utils.table_to_list(t)
	local l = {}
	for _, v in pairs(t) do
		l[#l + 1] = v
	end
	return l
end

function utils.list_to_table(l, key)
	local t = {}
	for i = 1, #l do
		t[l[i][key]] = l[i]
	end
	return t
end

function utils.makeReadOnlyTable(initValue)
	local t = {}
	return setmetatable(t, {
		__index = initValue,
		__newindex = function() error("can't write readonly table") end
	})
end

function utils.tableCopy(t)
	local c = {}
	for k, v in pairs(t) do
		if type(v) == "table" then
			c[k] = utils.tableCopy(v)
		else
			c[k] = v
		end
	end
	return c
end

function utils.table_merge(t, x)
	for k, v in pairs(x) do
		t[k] = v
	end
end

function utils.array_merge(t, x)
	for _, v in pairs(x) do
		t[#t + 1] = v
	end
end

function utils.table_remove(t, x)
	local equal = x
	if type(x) ~= "function" then
		equal = function(v) return v == x end
	end
	local is_deleted = false
	for k, v in pairs(t) do
		if equal(v) then
			t[k] = nil
			is_deleted = true
		end
	end
	return is_deleted
end

function utils.table_fill_with_default(t, default)
	if type(t) == "table" and type(default) == "table" then
		for k, v in pairs(default) do
			if t[k] == nil then
				if type(v) == "table" then
					t[k] = utils.tableCopy(v)
				else
					t[k] = v
				end
			end
		end
	end
end

function utils.list_remove(t,index,count)
	count = count or 1
	if index <= 0 or index > #t then
		return nil
	end
	local len = #t
	for i = index,len do
		t[i]=t[i+count]
	end
end

function utils.table_length(t)
	local n = 0
	for _, v in pairs(t) do
		n = n + 1
	end
	return n
end

function utils.utf8len(input)
    local len  = string.len(input)
    local left = len
    local cnt  = 0
    local arr  = {0, 0xc0, 0xe0, 0xf0, 0xf8, 0xfc}
    while left ~= 0 do
        local tmp = string.byte(input, -left)
        local i   = #arr
        while arr[i] do
            if tmp >= arr[i] then
                left = left - i
                break
            end
            i = i - 1
        end
        cnt = cnt + 1
    end
    return cnt
end

-- Find an element in table.
-- If x is a function it will pass the value to it to determine whether
-- they are equal. otherwise x will be compared to the value.
function utils.table_find(t, x)
	local equal = x
	if type(x) ~= "function" then
		equal = function(v) return v == x end
	end
	for _, v in pairs(t) do
		if equal(v) then
			return v
		end
	end
end

function utils.sum_if(t, x)
	local equal = x
	if type(x) ~= "function" then
		equal = function(v) return v == x end
	end
	local sum = 0
	for _, v in pairs(t) do
		if equal(v) then
			sum = sum + 1
		end
	end
	return sum
end

-- Return a sub-table.
function utils.array_sub(t, s, e)
	e = e or #t
	local ret = {}
	for i=s, e do
		ret[#ret + 1] = t[i]
	end
	return ret
end

function utils.read_file(filename)
	local file, err = io.open(filename, "rb")
	if not file then
		return nil, err
	end
	local data = file:read("*a")
	file:close()
	return data
end

function utils.str_split(str, sep)
	local fields = {}
	local pattern = strfmt("([^%s]+)", sep)
	str:gsub(pattern, function(c) fields[#fields+1] = c end)
	return fields
end

function utils.str_replace(str, what, replace)
	local s, n = str:gsub("%"..what, replace)
	if n > 0 then
		return s
	end
	return str
end

function utils.min(list, less_func)
	local min
	for _, v in pairs(list) do
		if min then
			if less_func(v, min) then
				min = v
			end
		else
			min = v
		end
	end
	return min
end

function utils.unixtime_from_string(s, fmt)
	if not fmt then
		fmt = "(%d+)%-(%d+)%-(%d+) (%d+):(%d+):(%d+)"
	end
	local year, month, day, hour, min, sec = s:match(fmt)
	return ostime{year=year,month=month,day=day,hour=hour,min=min,sec=sec}
end

function utils.unixtime_to_string(t, fmt)
	if not fmt then
		fmt = "%Y-%m-%d %H:%M:%S"
	end
	return osdate(fmt, t)
end

function utils.run_coroutine_safe(fn, errfn, ...)
	local safe_run = function(...)
		local ok, err = xpcall(fn, debug.traceback, ...)
		if not ok then
			errfn(err)
		end
	end
	local co = coroutine.create(safe_run)
	local ok, err = coroutine.resume(co, ...)
	if not ok then
		errfn(err)
	end
end

function utils.get_random_value(listWeight,allWeight)
	local randomWeight = math.random(1,allWeight)
	for _,r in pairs(listWeight) do 
		if r.k >= randomWeight then
			return r.v
		end
	end
end

function utils.itemMerge(t1,t2)
	for _,r2 in pairs(t2) do
		local isExist = false
		for _,r1 in pairs(t1) do 
			if r2.id == r1.id then
				isExist = true
				r1.count = r1.count + r2.count
				break
			end
		end
		if not isExist then
			table.insert(t1,utils.tableCopy(r2))
		end
	end
	return t1
end

function utils.get_random_table(t)
	local listWeight = {}
	local len = #t / 2
	local allWeight = 0
	if (#t % 2)==0 then
		for i = 1, len do 
			allWeight = allWeight + t[2*i]
			listWeight[i] = {k=allWeight,v=t[2*i-1]}
		end
		return listWeight,allWeight,len
	else
		return nil
	end
end

function utils.get_random_table_object(t)
	local len = #t / 3
	local result = {}
	if (#t % 3)==0 then
		for i = 1, len do 
			if math.random(1,100) <= t[3*i] then
				table.insert(result,{id=t[3*i-2],count=t[3*i-1]})
			end
		end
	end
	return result
end

function utils.get_passed_days(now, last_time, reset_hour)
	local is_at_next_day = last_time:hour() >= reset_hour
	last_time:set{hour=reset_hour, min=0, sec=0}
	local next_reset_time = last_time:to_unix() + (is_at_next_day and 3600*24 or 0)
	local now_unix = now:to_unix()
	local days = 0
	while now_unix > next_reset_time do
		days = days + 1
		next_reset_time = next_reset_time + 3600*24
	end
	return days
end

function utils.serialize(obj)
    local result = ""  
    local t = type(obj)  
    if t == "number" then  
        result = result .. obj  
    elseif t == "boolean" then  
        result = result .. tostring(obj)  
    elseif t == "string" then
        result = result .. string.format("%q", obj)
    elseif t == "table" then
        result = result .. "{\n"
        for k, v in pairs(obj) do  
            result = result .. "[" .. utils.serialize(k) .. "]=" .. utils.serialize(v) .. ",\n"  
        end  
        result = result .. "}"  
    elseif t == "nil" then
        return "nil"
    else
        return nil, "unknown value : " .. t
    end
    return result
end

local function PrintTable( tbl , level, filteDefault)
if type(tbl)~="table" then 
	print(tbl)
	return 
end
  local msg = ""
  filteDefault = filteDefault or true --默认过滤关键字（DeleteMe, _class_type）
  level = level or 1
  local indent_str = ""
  for i = 1, level do
    indent_str = indent_str.."  "
  end

  print(indent_str .. "{")
  for k,v in pairs(tbl) do
    if filteDefault then
      if k ~= "_class_type" and k ~= "DeleteMe" then
        local item_str = string.format("%s%s = %s", indent_str .. " ",tostring(k), tostring(v))
        print(item_str)
        if type(v) == "table" then
          utils.PrintTable(v, level + 1)
        end
      end
    else
      local item_str = string.format("%s%s = %s", indent_str .. " ",tostring(k), tostring(v))
      print(item_str)
      if type(v) == "table" then
        utils.PrintTable(v, level + 1)
      end
    end
  end
  print(indent_str .. "}")
end

utils.PrintTable = PrintTable


--the sum of wights is 1
function utils.choose_one_random_in_wights_array(wights, is_normialize)
	if not is_normialize then
		local sum = 0
		for i=1,#wights do
			sum = sum + wights[i]
		end
		for i=1,#wights do
			wights[i] = wights[i]/sum
		end
	end
	local random_num = math.random()
	--print("random:",random_num)
	local fragment_start = 0
	local fragment_end = 0
	local index
	for i=1,#wights do
		fragment_end = fragment_start + wights[i]
		if random_num>=fragment_start and random_num<fragment_end then
            index = i
            break
	    end
	    fragment_start = fragment_end
	end
    return index
end

function utils.encode_url(s)
    s = string.gsub(s, "([^%w%.%- ])", function(c) return string.format("%%%02X", string.byte(c)) end)
    return string.gsub(s, " ", "+")
end

function utils.decode_url(s)
    s = string.gsub(s, '%%(%x%x)', function(h) return string.char(tonumber(h, 16)) end)
    return s
end

function utils.unserialize(str)
    local t = type(str)  
    if t == "nil" or str == "" then  
        return nil, "no data"
    elseif t == "number" or t == "string" or t == "boolean" then
        str = tostring(str)  
    else  
        return nil, "unknown value : " .. t
    end  
    str = "return " .. str  
    local func = load(str)  
    if func == nil then  
        return nil, "error data"
    end  
    return func()
end  

return utils