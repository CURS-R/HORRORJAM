using System;
using System.Collections.Generic;
using CURSR.Network;
using Fusion;
using UnityEngine;
using CURSR.Settings;
using Fusion.Sockets;

namespace CURSR.Game
{
    public class Player : NetworkBehaviour, INetworkRunnerCallbacks
    {
        [field:SerializeField] private GameContainer gameContainer;
        [field:SerializeField] private SettingsContainer settingsContainer;
        [field:Header("Unity components")]
        [field:SerializeField] private CharacterController localCC { get; set; }
        [field:SerializeField] private Transform localViewTransform { get; set; }
        
        // Networked
        [Networked] private ref PlayerData PlayerData => ref MakeRef<PlayerData>();

        private PlayerCharacterController playerCharacterController;
        private PlayerViewController playerViewController;

        public override void Spawned()
        {
            playerCharacterController ??= new(localCC, settingsContainer.PlayerSettings.PlayerMovementSettings);
            playerViewController ??= new(localViewTransform, settingsContainer.PlayerSettings.PlayerViewSettings);
            ToggleControllersBasedOnAuthority();
            
            Runner.AddCallbacks(this);
            
            base.Spawned();
        }
        
        public void OnDisable()
        {
            if(Runner != null)
            {
                Runner.RemoveCallbacks(this);
            }
        }
        
        public void ToggleControllersBasedOnAuthority()
        {
            if (HasInputAuthority)
            {
                localCC.enabled = true;
                playerCharacterController.Enable();
                playerViewController.Enable();
            }
            else
            {
                localCC.enabled = false;
                playerCharacterController.Disable();
                playerViewController.Disable();
            }
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            if (HasInputAuthority)
            {
                var outgoing = new PlayerInputStruct{
                    ccPosition = localCC.transform.position,
                    ccRotation = localCC.transform.rotation,
                    viewRotation = localViewTransform.rotation,
                };
                input.Set(outgoing);
            }
        }
        
        public override void FixedUpdateNetwork()
        {
            if (GetInput(out PlayerInputStruct incoming))
            {
                PlayerData.CCPosition = incoming.ccPosition;
                PlayerData.CCRotation = incoming.ccRotation;
                PlayerData.viewPoint = incoming.viewRotation;
            }
        }

        public void Update()
        {
            if (HasInputAuthority)
            {
                //if (Cursor.lockState != CursorLockMode.Locked)
                //{
                    playerViewController.Process();
                    playerCharacterController.ProcessRotate(playerViewController.GetPitch(), Time.deltaTime);
                //}
                playerCharacterController.ProcessMove(Time.deltaTime);
            }
            else
            {
                localCC.transform.position = PlayerData.CCPosition;
                localCC.transform.rotation = PlayerData.CCRotation;
                localViewTransform.rotation = PlayerData.viewPoint;
            }
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player){}
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player){}
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){}
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason){}
        public void OnConnectedToServer(NetworkRunner runner){}
        public void OnDisconnectedFromServer(NetworkRunner runner){}
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token){}
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason){}
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){}
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){}
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){}
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){}
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data){}
        public void OnSceneLoadDone(NetworkRunner runner){}
        public void OnSceneLoadStart(NetworkRunner runner){}
    }
    
    [System.Serializable]
    public struct PlayerData : INetworkStruct
    {
        public int ownerID { get; set; }
        // TODO: more networked playerdata
        // Visual/Movement
        public Vector3 CCPosition { get; set; }
        public Quaternion CCRotation { get; set; }
        public Quaternion viewPoint { get; set; }
    }

    public struct PlayerInputStruct : INetworkInput
    {
        public Vector3 ccPosition { get; set; }
        public Quaternion ccRotation { get; set; }
        public Quaternion viewRotation { get; set; }
    }
}