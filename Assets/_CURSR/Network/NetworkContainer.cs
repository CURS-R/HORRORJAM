using System;
using System.Collections;
using System.Collections.Generic;
using CURSR.Utils;
using Fusion;
using UnityEngine;
using CTU = CURSR.Utils.ConnectionTokenUtils;

namespace CURSR.Network
{
    [CreateAssetMenu(fileName = "NetworkContainer", menuName = "CURSR/Container/Network")]
    public class NetworkContainer : ScriptableObject
    {
        // Serialized
        [field: Header("Prefabs")]
        [field:SerializeField] public NetworkRunner RunnerPrefab { get; private set; }
        
        // Properties
        private byte[] _localConnectionToken;
        public byte[] localConnectionToken
        {
            get => _localConnectionToken;
            set
            {
                Log($"LocalConnectionToken changed to (hashed): {CTU.HashToken(value)}");
                _localConnectionToken = value;
            }
        }

        public HashSet<byte[]> ConnectionTokens { get; set; }

        public event Action<NetworkRunner> LocalJoinRoomEvent;
        public void InvokeLocalJoinRoomEvent(NetworkRunner runner) => LocalJoinRoomEvent?.Invoke(runner);

        public event Action<NetworkRunner, PlayerRef> PlayerJoinRoomEvent;
        public void InvokePlayerJoinRoomEvent(NetworkRunner runner, PlayerRef playerRef) => PlayerJoinRoomEvent?.Invoke(runner, playerRef);
        public event Action<NetworkRunner, PlayerRef> PlayerLeaveRoomEvent;
        public void InvokePlayerLeaveRoomEvent(NetworkRunner runner, PlayerRef playerRef) => PlayerLeaveRoomEvent?.Invoke(runner, playerRef);

        private void Log(string message) => Debug.Log(message, this);
    }
}
