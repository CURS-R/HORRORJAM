using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using CURSR.Visual;
using CURSR.Game;
using CURSR.Network;
using CURSR.Map;
using CURSR.Input;
using CURSR.Saves;
using CURSR.Settings;
using CURSR.Utils;
using TMPro;

namespace CURSR.Display
{
    public class Hotbar : MonoBehaviour
    {
        [Header("Containers")]
        [field:SerializeField] private GameContainer gameContainer;
        [field:SerializeField] private SettingsContainer settingsContainer;
        
        [Header("Components")]
        [field:SerializeField] private TMP_Text itemLabelTMP;
        
        [field:SerializeField] private ItemDisplay itemDisplayPrefab;
        [field:SerializeField] private Transform itemDisplaysClonePoint;

        private void Awake()
        {
            ResetHotbar();
        }

        public void ResetHotbar()
        {
            // Label
            itemLabelTMP.text = "";
            // ItemDisplays
            itemDisplaysPool ??= new();
            foreach (var itemDisplay in itemDisplaysPool)
                Destroy(itemDisplay.gameObject);
            itemDisplaysPool.Clear();
            for (int i = 0; i < settingsContainer.PlayerSettings.PlayerHotbarSettings.MaxHotbarItems; i++)
                itemDisplaysPool.Add(Instantiate(itemDisplayPrefab, itemDisplaysClonePoint));
        }

        private List<ItemDisplay> itemDisplaysPool;
        
        public void SetHotbarItemDisplay(Item item, int index)
        {
            itemDisplaysPool[index].itemBind = item;
        }

        public void SelectItemInHotbar(int index)
        {
            foreach (var itemDisplay in itemDisplaysPool) 
                itemDisplay.SelectionVisibility = false;
            itemDisplaysPool[index].SelectionVisibility = true;
            if (itemDisplaysPool[index].itemBind != null)
                itemLabelTMP.text = itemDisplaysPool[index].itemBind.ItemSO.name;
        }
    }
}