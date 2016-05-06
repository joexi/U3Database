using UnityEngine;
using System.Collections;


public class U3DBLog
{

    public static void Log(string message)
    {
        UnityEngine.Debug.Log("[U3DB][Log]" + message);
    }

    public static void Info(string message)
    {
        UnityEngine.Debug.Log("[U3DB][Info]" + message);
    }

    public static void LogWarning(string message)
    {
        UnityEngine.Debug.Log("[U3DB][Warning]" + message);
    }

    public static void LogError(string message)
    {
        UnityEngine.Debug.LogError("[U3DB][Error]" + message);
    }

}