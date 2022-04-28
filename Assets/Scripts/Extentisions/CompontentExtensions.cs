using UnityEngine;

public static class CompontentExtensions
{
    public static bool TryGetComponent<T>(this Component component, out T t)
    {
        t = component.GetComponent<T>();
        return t != null;
    }
}