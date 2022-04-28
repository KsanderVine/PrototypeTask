using UnityEngine;

public static class CompontentExtensions
{
    public static bool TryGetComponent<T>(this Component parentComponent, out T component)
    {
        component = parentComponent.GetComponent<T>();
        return component != null;
    }
}