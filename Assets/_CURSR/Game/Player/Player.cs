﻿using System;
using System.Collections.Generic;
using System.Linq;
using CURSR.Network;
using Fusion;
using UnityEngine;
using CURSR.Settings;
using CURSR.Utils;
using Fusion.Sockets;

namespace CURSR.Game
{
    public class Player : NetworkBehaviour, INetworkRunnerCallbacks
    {
        [field:SerializeField] private GameContainer gameContainer;
        [field:SerializeField] private SettingsContainer settingsContainer;
        [field:Header("Unity components")]
        [field:SerializeField] public CharacterController localCC { get; private set; }
        [field:SerializeField] public Transform localViewTransform { get; private set; }
        [field:SerializeField] public LayerMask layerMaskForLocalPlayer { get; private set; }
        
        [Networked]
        [HideInInspector] public ref PlayerData PlayerData => ref MakeRef<PlayerData>();
        
        [Networked, Capacity(32), UnitySerializeField]
        [HideInInspector] public NetworkLinkedList<int> Inventory { get; }
        [HideInInspector] public List<ItemSO> Items => Inventory.Select(itemIndex => gameContainer.ItemsRegistry.Items[itemIndex]).ToList();
        private int selectedItem;
        
        public event Action<int> ChangeItemSelection;
        public void InvokeChangeItemSelection(int itemSelection) => ChangeItemSelection?.Invoke(itemSelection);

        private PlayerCharacterController playerCharacterController;
        private PlayerViewController playerViewController;
        private PlayerInteractionController playerInteractionController;
        private PlayerInventoryController playerInventoryController;
        
        private LayerMask initialLayerMask;

        private void Awake()
        {
            initialLayerMask = 1 << this.gameObject.layer;
        }

        public override void Spawned()
        {
            SetupControllers();
            
            Runner.AddCallbacks(this);

            if (HasInputAuthority)
                gameContainer.InvokeLocalPlayerSpawnedEvent(this);
            else
                gameContainer.InvokeOtherPlayerSpawnedEvent(this);
            
            base.Spawned();
        }
        
        public void OnDisable()
        {
            if(Runner != null)
            {
                Runner.RemoveCallbacks(this);
            }
        }

        private void SetupControllers()
        {
            playerCharacterController ??= new(settingsContainer.PlayerSettings.PlayerMovementSettings, localCC);
            playerViewController ??= new(settingsContainer.PlayerSettings.PlayerViewSettings, localViewTransform);
            playerInteractionController ??= new(settingsContainer.PlayerSettings.PlayerInteractionSettings, localViewTransform);
            playerInventoryController ??= new();
            if (HasInputAuthority)
            {
                localCC.enabled = true;
                playerCharacterController.Enable();
                playerViewController.Enable();
                playerInteractionController.Enable();
                playerInventoryController.Enable();
                GameObjectUtil.ChangeLayerRecursively(this.transform, layerMaskForLocalPlayer);
            }
            else
            {
                localCC.enabled = false;
                playerCharacterController.Disable();
                playerViewController.Disable();
                playerInteractionController.Enable();
                playerInventoryController.Enable();
                GameObjectUtil.ChangeLayerRecursively(this.transform, initialLayerMask);
            }
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            if (!HasInputAuthority) return;
            
            var outgoing = new PlayerInputStruct{
                ccPosition = localCC.transform.position,
                ccRotation = localCC.transform.rotation,
                viewRotation = localViewTransform.rotation,
            };
            
            input.Set(outgoing);
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
                // TODO: Cursor Lockstate
                //if (Cursor.lockState != CursorLockMode.Locked)
                //{
                    playerViewController.Process();
                    playerCharacterController.ProcessRotate(playerViewController.GetPitch(), Time.deltaTime);
                //}
                playerCharacterController.ProcessMove(Time.deltaTime);
                playerInteractionController.Process(Time.deltaTime);
                playerInventoryController.Process(Time.deltaTime);
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