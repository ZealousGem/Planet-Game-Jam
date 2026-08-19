using UnityEngine;
using System;
using System.Collections.Generic;

public static class EventBus
{

    private static Dictionary<Type, Delegate> SubbedActions = new();

    public static void Act(EventData data)
    {

        Type newType = data.GetType();

        // 
        foreach (var k in SubbedActions)
        {
            if (k.Key.IsAssignableFrom(newType))
            {
                k.Value?.DynamicInvoke(data);
            }
        }

    }

    public static void Subscribe<T>(Action<T> act) where T : EventData
    {
        Type type = typeof(T);

        if (SubbedActions.ContainsKey(type))
        {
            SubbedActions[type] = Delegate.Combine(SubbedActions[type], act);
        }

        else
        {
            SubbedActions[type] = act;
        }
    }

    public static void Unsubscribe<T>(Action<T> act) where T : EventData
    {
        Type type = typeof(T);

        if (SubbedActions.ContainsKey(type))
        {

            SubbedActions[type] = Delegate.Remove(SubbedActions[type], act);

        }


    }



}