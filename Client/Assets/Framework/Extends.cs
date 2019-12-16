using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class Extends {

	public static T GetCustomAttribute<T>(this FieldInfo fieldInfo) where T:Attribute
    {
        var customAttributes = fieldInfo.GetCustomAttributes(true);
        foreach (var attribute in customAttributes) {
            if(attribute.GetType() == typeof(T))
            {
                return attribute as T;
            }
        }
        return null;
    }
}
