using UnityEngine;
using System.Collections;

public static class Utility {

	public static T QueryComponent<T>(this GameObject obj) where T : Component
    {
        var com = obj.GetComponent<T>();
        if (com == null)
            com = obj.AddComponent<T>();
        return com;
    }
}
