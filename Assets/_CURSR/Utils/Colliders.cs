using System.Collections.Generic;
using UnityEngine;

namespace CURSR.Utils
{
    public static class Colliders
    {
        public static T GetComponentUpwards<T>(GameObject obj) where T : Component
        {
            var t = obj.transform;

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
        
        public static List<Collider> GetAllColliders(GameObject obj)
        {
            List<Collider> colliders = new();
            GetAllCollidersRecursive(obj.transform, colliders);
            return colliders;
        }

        private static void GetAllCollidersRecursive(Transform t, List<Collider> colliders)
        {
            var localColliders = t.GetComponents<Collider>();
            colliders.AddRange(localColliders);

            foreach (Transform child in t)
            {
                GetAllCollidersRecursive(child, colliders);
            }
        }
    }
}