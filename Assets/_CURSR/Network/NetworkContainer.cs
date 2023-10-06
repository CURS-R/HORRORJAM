using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace CURSR.Network
{
    [CreateAssetMenu(fileName = "NetworkContainer", menuName = "CURSR/Container/Network")]
    public class NetworkContainer : ScriptableObject
    {
        #region FIELDS
        
        // Serialized
        [field: Header("Prefabs")]
        [field:SerializeField] public NetworkRunner RunnerPrefab { get; private set; }
        
        #endregion
        
        // MAYBE
        //[HideInInspector] public int localPlayerIdentity;

        public event Action<NetworkRunner> JoinRoomEvent;
        public void InvokeJoinRoomEvent(NetworkRunner runner) => JoinRoomEvent?.Invoke(runner);
        public event Action<NetworkRunner> HostMigrationEvent;
        public void InvokeHostMigrationEvent(NetworkRunner newRunner) => HostMigrationEvent?.Invoke(newRunner);
        
        
        
        public event Action<PlayerRef, int> PlayerJoinEvent;
        public void TryInvokePlayerJoinEvent(PlayerRef playerRef, int playerToken) => PlayerJoinEvent?.Invoke(playerRef, playerToken);
        public event Action<NetworkBehaviour> PlayerLeaveEvent;
        public void TryInvokePlayerLeaveEvent(NetworkBehaviour monoBehaviour) => PlayerLeaveEvent?.Invoke(monoBehaviour);
    }
}
