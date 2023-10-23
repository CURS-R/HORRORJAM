using UnityEngine;

namespace CURSR.Utils
{
    public static class Colliders
    {
        public static T GetComponentUpwards<T>(GameObject obj) where T : Component
        {
            Transform t = obj.transform;

            while (t != null)
            {
                T component = t.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
                t = t.parent;
            }

            return null;
        }
    }
}