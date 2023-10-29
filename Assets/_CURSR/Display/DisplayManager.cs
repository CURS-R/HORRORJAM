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
        [Header("Containers")]
        [field:SerializeField] private VisualContainer visualContainer;
        [field:SerializeField] private DisplayContainer displayContainer;
        [field:SerializeField] private GameContainer gameContainer;
        [field:SerializeField] private NetworkContainer networkContainer;
        [field:SerializeField] private MapContainer mapContainer;
        [field:SerializeField] private SavesContainer savesContainer;
        [field:SerializeField] private SettingsContainer settingsContainer;
        
        [Header("Components")]
        [field:SerializeField] private Hotbar hotbar;
        
        protected override void Init() { }

        private void OnEnable()
        {
            gameContainer.LocalPlayerSpawnedEvent += BindToPlayer;
        }

        private Player boundPlayer;
        private void BindToPlayer(Player player)
        {
            boundPlayer = player;
            player.ChangeHotbarSelection += hotbar.SelectItemDisplay;
            player.PickupItem += hotbar.BindItem;
            player.DropItem += hotbar.UnbindItem;
            // TODO: item hovering display
            //player.HoverOverItem += 
            //player.UnHoverOverItem +=
            hotbar.ResetHotbar();
        }
    }
}