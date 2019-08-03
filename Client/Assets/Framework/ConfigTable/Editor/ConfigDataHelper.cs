using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace bluebean.UGFramework.ConfigData
{
    public static class ConfigDataHelper
    {

        public static bool IsValidVariableName(string name)
        {
            Regex re = new Regex(@"^[a-zA-Z_][a-zA-Z0-9_]*$");
            return re.IsMatch(name);
        }

    }
}
