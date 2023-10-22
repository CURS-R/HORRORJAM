using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CURSR.Game
{
    [CreateAssetMenu(fileName = "ItemsRegistry", menuName = "CURSR/Game/Inventory/ItemsRegistry")]
    public class ItemsRegistry : ScriptableObject
    {
        [field: SerializeField] public List<ItemSO> Items { get; private set; }
        
        // LATER: be smart and just get rid of these variables lol
        List<ItemSO> BaseItems => Items.Where(item => item.category == ItemCategory.BASE).ToList();
        List<ItemSO> GearItems => Items.Where(item => item.category == ItemCategory.GEAR).ToList();
        List<ItemSO> ExtraItems => Items.Where(item => item.category == ItemCategory.EXTRA).ToList();
    }
}
