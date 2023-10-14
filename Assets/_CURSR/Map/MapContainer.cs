using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using CURSR.Input;
using CURSR.Saves;
using CURSR.Settings;
using CURSR.Utils;

namespace CURSR.Network
{
    [CreateAssetMenu(fileName = "MapContainer", menuName = "CURSR/Container/Map")]
    public class MapContainer : ScriptableObject
    {
        // TODO: MapContainer
        /*
        [field: Header("Containers")]
        [field:SerializeField] public SettingsContainer SettingsContainer { get; private set; }

        // Serialized
        [field: Header("Prefabs")]
        [field:SerializeField] public Map MapPrefab { get; private set; }


        private Map _game;
        public Map Game
        {
            get => _game;
            set
            {
                Log($"Game changed.");
                _game = value;
            }
        }

        public event Action<Map> MapSpawnedEvent;
        public void InvokePlayerSpawnedEvent(Map map) => MapSpawnedEvent?.Invoke(map);
        */
        
        private void Log(string message) => Debug.Log(message, this);
    }
}