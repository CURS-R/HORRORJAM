using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CURSR.Game
{
    [CreateAssetMenu(fileName = "NewItemSO", menuName = "CURSR/Game/Inventory/Item")]
    public class ItemSO : ScriptableObject
    {
        [field:SerializeField] public string name { get; private set; }
        [field:SerializeField] public ItemCategory category { get; private set; }
        [field:SerializeField] public Sprite icon { get; private set; }
        [field:SerializeField] public GameObject visualGameObject { get; private set; }
    }

    public enum ItemCategory
    {
        BASE,
        GEAR,
        EXTRA,
    }
}
