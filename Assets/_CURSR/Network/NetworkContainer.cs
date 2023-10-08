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
            get
            {
                return _localConnectionToken;
            }
            set
            {
                Log("LocalConnectionToken changed to (hashed): " + CTU.HashToken(value));
                _localConnectionToken = value;
            }
        }
        public HashSet<byte[]> otherPlayerTokens;

        public event Action CreateRoomEvent;
        public void InvokeCreateRoomEvent() => CreateRoomEvent?.Invoke();
        public event Action JoinRoomEvent;
        public void InvokeJoinRoomEvent() => JoinRoomEvent?.Invoke();

        public event Action<PlayerRef> PlayerJoinEvent;
        public void TryInvokePlayerJoinEvent(PlayerRef playerRef) => PlayerJoinEvent?.Invoke(playerRef);
        public event Action<PlayerRef> PlayerLeaveEvent;
        public void TryInvokePlayerLeaveEvent(PlayerRef playerRef) => PlayerLeaveEvent?.Invoke(playerRef);

        private void Log(string message) => Debug.Log(message, this);
    }
}
