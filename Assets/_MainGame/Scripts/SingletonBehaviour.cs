
namespace FarmGame
{
    using UnityEngine;

    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T m_Current;
        public static T Current
        {
            get
            {
                if (m_Current != null) return m_Current;

                var instances = FindObjectsOfType<T>();

                // Only found 1
                if (instances.Length == 1)
                {
                    m_Current = instances[0];
                    return m_Current;
                }

                string s1 = $"<b>[{nameof(T)}]</b>";

                // Found nothing
                if (instances.Length == 0)
                {
                    var gObj = new GameObject(nameof(T) + " (Runtime)");
                    m_Current = gObj.AddComponent<T>();
                    Debug.LogWarning($"No instance of type {s1} found, creating runtime instance. This could lead to some bug!", gObj);
                    return m_Current;
                }

                // Found more than one
                m_Current = instances[0];
                Debug.LogWarning($"More than 1 instance of type {s1} found. Will use the first one in the list", instances[0].gameObject);
                return m_Current;
            }
        }
    }
}

