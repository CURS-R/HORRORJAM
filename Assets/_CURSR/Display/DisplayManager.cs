using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using CURSR.Visual;
using CURSR.Game;
using CURSR.Network;
using CURSR.Map;
using CURSR.Input;
using CURSR.Saves;
using CURSR.Settings;
using CURSR.Utils;

namespace CURSR.Display
{
    public class DisplayManager : Singleton<DisplayManager>
    {
        [field:SerializeField] private VisualContainer visualContainer;
        [field:SerializeField] private DisplayContainer displayContainer;
        [field:SerializeField] private GameContainer gameContainer;
        [field:SerializeField] private NetworkContainer networkContainer;
        [field:SerializeField] private MapContainer mapContainer;
        [field:SerializeField] private SavesContainer savesContainer;
        [field:SerializeField] private SettingsContainer settingsContainer;
        protected override void Init()
        {
            UpdateHotbarItemDisplays();
        }

        private void OnEnable()
        {
            gameContainer.LocalPlayerSpawnedEvent += BindToPlayer;
        }

        private Player boundPlayer;
        private void BindToPlayer(Player player)
        {
            boundPlayer = player;
            player.ChangeItemSelection += SetHotbarItemDisplay;
        }
        
        [field:SerializeField] private List<ItemDisplay> ItemDisplays;

        private int _hotbarIndex;

        private int HotbarIndex
        {
            get
            {
                return _hotbarIndex;
            }
            set
            {
                _hotbarIndex = Mathf.Clamp(value, 0, ItemDisplays.Count);
                UpdateHotbarItemDisplays();
            }
        }

        private void SetHotbarItemDisplay(int index)
        {
            var itemSprite = gameContainer.ItemsRegistry.Items[index].icon;
            // TODO:
        }

        private void UpdateHotbarItemDisplays()
        {
            foreach (var itemDisplay in ItemDisplays) 
                itemDisplay.SelectionVisibility = false;
            ItemDisplays[HotbarIndex].SelectionVisibility = true;
        }


    }
}