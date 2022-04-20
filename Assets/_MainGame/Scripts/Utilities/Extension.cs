namespace FarmGame
{
    using UnityEngine;

    public static class Extension
    {
        public static bool TryNullCheckAndLog<T, TTrace>(this T target, string message, TTrace trace)
        where T : Object
        where TTrace : Object
        {
            if (target == null && trace == null)
            {
                Debug.LogWarning($"<b>[Null]</b>Parameters are null with this message: {message}");
                return true;
            }
            if (target == null)
            {
                Debug.LogWarning($"<b>[Null]</b>{message} (click to trace {trace.name})", trace);
                return true;
            }

            return false;
        }
    }
}

