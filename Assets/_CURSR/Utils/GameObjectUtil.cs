using UnityEngine;

namespace CURSR.Utils
{
    public static class GameObjectUtil
    {
        public static void ChangeLayerRecursively(Transform root, int layer)
        {
            root.gameObject.layer = layer;
            foreach (Transform child in root) ChangeLayerRecursively(child, layer);
        }
        public static void ChangeLayerRecursively(Transform root, LayerMask layerMask)
        {
            int layerIndex = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2)); // Convert LayerMask to layer index
            ChangeLayerRecursively(root, layerIndex);
        }
    }
}