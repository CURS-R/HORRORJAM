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
        // Serialized
        [field: Header("Prefabs")]
        [field:SerializeField] public Game GamePrefab { get; private set; }
        [field:SerializeField] public Player PlayerPrefab { get; private set; }

        [HideInInspector] public Game Game;

        public event Action<Player> OtherPlayerSpawnedEvent;
        public void InvokeOtherPlayerSpawnedEvent(Player player) => OtherPlayerSpawnedEvent?.Invoke(player);
        public event Action<Player> LocalPlayerSpawnedEvent;
        public void InvokeLocalPlayerSpawnedEvent(Player player) => LocalPlayerSpawnedEvent?.Invoke(player);
        public event Action<Enemy> EnemySpawnedEvent;
        public void InvokeEnemySpawnedEvent(Enemy enemy) => EnemySpawnedEvent?.Invoke(enemy);

        private void Log(string message) => Debug.Log(message, this);
    }
}