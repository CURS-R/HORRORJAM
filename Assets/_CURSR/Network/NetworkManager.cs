using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using CURSR.Utils;
using Random = UnityEngine.Random;
using CTU = CURSR.Utils.ConnectionTokenUtils;

namespace CURSR.Network
{
    /// <summary>
    /// The "guy" for networking.
    /// </summary>
    public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        [field:SerializeField] private NetworkContainer networkContainer;
        
        private NetworkRunner runnerPrefab => networkContainer.RunnerPrefab;
        private static NetworkRunner Runner { get; set; }

        private bool isHost => Runner.IsSinglePlayer || Runner.IsServer;
        
        #region TESTING
        private void OnGUI()
        {
            if (Runner == null)
            {
                if (GUI.Button(new Rect(0,0,200,40), "Singleplayer"))
                {
                    StartOrJoinRoom(GameMode.Single);
                }
                if (GUI.Button(new Rect(0,40,200,40), "AutoHostJoin"))
                {
                    StartOrJoinRoom(GameMode.AutoHostOrClient);
                }
                if (GUI.Button(new Rect(420,0,200,40), "JoinSpecificRoom"))
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                if (GUI.Button(new Rect(0,0,200,40), "Disconnect"))
                {
                    LeaveRoom();
                }
            }
        }
        #endregion
        
        protected void Awake()
        {
            // TODO: get connection token elsewhere?
            networkContainer.localConnectionToken = CTU.NewToken(true);
        }

        public async void StartOrJoinRoom(GameMode mode)
        {
            networkContainer.OtherClientTokens = new();

            InstantiateRunner();
            
            await Runner.StartGame(new()
                {
                    GameMode = mode,
                    SessionName = "TestRoom",
                    Scene = SceneManager.GetActiveScene().buildIndex,
                    ConnectionToken = networkContainer.localConnectionToken
                }
            );
            
            Debug.Log("StartOrJoinRoom finished.");
        }
        
        public async void LeaveRoom()
        {
            await Runner.Shutdown();
            Runner = null;
            
            // TODO: better handling of this
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        #region Runner Callbacks
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef playerRef)
        {
            if (isHost)
            {
                byte[] token = runner.GetPlayerConnectionToken(playerRef) ?? networkContainer.localConnectionToken;
                Log($"OnPlayerJoined fired, using token (hashed): {CTU.HashToken(token)}");

                if (!networkContainer.OtherClientTokens.Add(token))
                    Log($"{CTU.HashToken(token)} was found in the HashSet. Attempting PlayerGB assignment.");
                else
                    Log($"No found value for token {CTU.HashToken(token)}.");
            }

            networkContainer.InvokePlayerJoinRoomEvent(runner, playerRef);
        }
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef playerRef) {}
        public void OnInput(NetworkRunner runner, NetworkInput input) {}
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {}
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        // ReSharper disable once Unity.IncorrectMethodSignature
        public void OnConnectedToServer(NetworkRunner runner)
        {
            if (isHost)
            {
                networkContainer.InvokeCreateRoomEvent(runner);
            }

            networkContainer.InvokeJoinRoomEvent(runner);
        }
        public void OnDisconnectedFromServer(NetworkRunner runner) {}
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) {}
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) {}
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) {}
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) {}
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) {}
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) {}
        public void OnSceneLoadDone(NetworkRunner runner) {}
        public void OnSceneLoadStart(NetworkRunner runner) {}
        public async void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { await runner.Shutdown(shutdownReason: ShutdownReason.HostMigration, destroyGameObject:true); }
        #endregion
        
        private void InstantiateRunner()
        {
            Runner = Instantiate(runnerPrefab);
            Runner.ProvideInput = true;
            Runner.AddCallbacks(this);
        }
        
        private void Log(string message) => Debug.Log(message, this);
    }
}
