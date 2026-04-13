using System;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    private static Dictionary<string,List<Action<object>>> Listeners = new Dictionary<string, List<Action<object>>>();

    public static void AddListener(string name, Action<object> callback)
    {
        if (!Listeners.ContainsKey(name))
            Listeners.Add(name, new List<Action<object>>());

        Listeners[name].Add(callback);
    }

    public static void RemoveListener(string name, Action<object> callback)
    {
        if (!Listeners.ContainsKey(name))
            return;

        Listeners[name].Remove(callback);
    }

    public static void Notify(string name, object data)
    {
        if (!Listeners.ContainsKey(name))
            return;

        foreach (var item in Listeners[name])
        {
            try
            {
                item?.Invoke(data);
            }
            catch(Exception e)
            {
                Debug.LogError("Error on invoke: " + e);
            }
        }
    }
}