using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlackboardType
{
    Individual,
    Group,
}

public class Blackboard 
{
   public Dictionary<string, object> variables = new Dictionary<string, object>();
    

    public void UpdateBlackboard(string key, object value)
    {
        if (variables.ContainsKey(key))
        {
            variables[key] = value;
        } else
        {
            variables.Add(key, value);

        }


    }

    public void ClearBlackboard(string key)
    {
        variables.Remove(key);
    }
    public T GetValue<T>(string key)
    {
        // Check if value is present in the runner.
        object value;
        T result = default(T);
        if (variables.TryGetValue(key, out value))
        {
            // Make sure the type is valid.
            if (value.GetType().IsAssignableFrom(typeof(T)))
            {
               
                result = (T)value;
            }
            else
            {
                Debug.LogError(key + " is not assignable to " + typeof(T));
                result = default(T);
            }
        }
        // Check if value is the correct type.
        return result;
    }
   
   
}
