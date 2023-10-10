using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using CURSR.Game;
using CURSR.Input;
using CURSR.Network;
using CURSR.Saves;
using CURSR.Settings;
using CURSR.Utils;

namespace CURSR.Network
{
    [CreateAssetMenu(fileName = "GameContainer", menuName = "CURSR/Container/Game")]
    public class GameContainer : ScriptableObject
    {
        [field: Header("Containers")]
        //[field:SerializeField] public InputContainer InputContainer { get; private set; }
        [field:SerializeField] public NetworkContainer NetworkContainer { get; private set; }
        [field:SerializeField] public SavesContainer SavesContainer { get; private set; }
        [field:SerializeField] public SettingsContainer SettingsContainer { get; private set; }
        
        // Serialized
        [field: Header("Prefabs")]
        [field:SerializeField] public Game GamePrefab { get; private set; }
        [field:SerializeField] public Player PlayerPrefab { get; private set; }

        private GAMESTATE _gameState;
        public GAMESTATE GameState
        {
            get => _gameState;
            set
            {
                Log($"GameState changed to: {value}");
                _gameState = value;
            }
        }

        private Game _game;
        public Game Game
        {
            get => _game;
            set
            {
                Log($"Game changed.");
                _game = value;
            }
        }

        public event Action<Player> PlayerSpawnedEvent;
        public void InvokePlayerSpawnedEvent(Player player) => PlayerSpawnedEvent?.Invoke(player);
        public event Action<Player> EnemySpawnedEvent;
        public void EnemyPlayerSpawnedEvent(Player player) => PlayerSpawnedEvent?.Invoke(player);

        private void Log(string message) => Debug.Log(message, this);
    }
}