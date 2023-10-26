using System;
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
        
        [Networked, Capacity(10), UnitySerializeField]
        [HideInInspector] public NetworkLinkedList<int> HotbarItems { get; }
        [HideInInspector] public List<ItemSO> Items => HotbarItems.Select(itemIndex => gameContainer.ItemSOsRegistry.Items[itemIndex]).ToList();

        public event Action<int> ChangeHotbarSelection;
        public void InvokeChangeItemSelection(int itemSelection) => ChangeHotbarSelection?.Invoke(itemSelection);
        public event Action<Item> HoverOverItem;
        public void InvokeHoverOverItem(Item item) => HoverOverItem?.Invoke(item);
        public event Action UnHoverOverItem;
        public void InvokeUnHoverOverItem() => UnHoverOverItem?.Invoke();
        public event Action<Item, int> PickupItem;
        public void InvokePickupItem(Item item, int hotbarIndex) => PickupItem?.Invoke(item, hotbarIndex);
        public event Action<Item> UseItem;
        public void InvokeUseItem(Item item) => UseItem?.Invoke(item);
        public event Action<Item, int> DropItem;
        public void InvokeDropItem(Item item, int hotbarIndex) => DropItem?.Invoke(item, hotbarIndex);

        private PlayerCharacterController playerCharacterController;
        private PlayerViewController playerViewController;
        private PlayerInteractionController playerInteractionController;
        private PlayerHotbarController playerHotbarController;
        
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
                ProcessControllers();
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
        
        private void SetupControllers()
        {
            playerCharacterController ??= new(settingsContainer.PlayerSettings.PlayerMovementSettings, localCC);
            playerViewController ??= new(settingsContainer.PlayerSettings.PlayerViewSettings, localViewTransform);
            playerInteractionController ??= new(settingsContainer.PlayerSettings.PlayerInteractionSettings, localViewTransform);
            playerHotbarController ??= new(settingsContainer.PlayerSettings.PlayerHotbarSettings);
            if (HasInputAuthority)
            {
                localCC.enabled = true;
                playerCharacterController.Enable();
                playerViewController.Enable();
                playerInteractionController.Enable();
                playerHotbarController.Enable();
                GameObjectUtil.ChangeLayerRecursively(this.transform, layerMaskForLocalPlayer);
            }
            else
            {
                localCC.enabled = false;
                playerCharacterController.Disable();
                playerViewController.Disable();
                playerInteractionController.Enable();
                playerHotbarController.Enable();
                GameObjectUtil.ChangeLayerRecursively(this.transform, initialLayerMask);
            }
        }

        private void ProcessControllers()
        {
            // TODO: Cursor Lockstate
            //if (Cursor.lockState != CursorLockMode.Locked)
            //{
                playerViewController.Process();
                playerCharacterController.ProcessRotate(playerViewController.GetPitch(), Time.deltaTime);
            //}
            playerCharacterController.ProcessMove(Time.deltaTime);
            HandleInteractionControllerData(playerInteractionController.Process(Time.deltaTime));
            HandleHotbarControllerData(playerHotbarController.Process(Time.deltaTime));
        }

        private void HandleInteractionControllerData(PlayerInteractionControllerData data)
        {
            if (data.IsHovering)
            {
                InvokeHoverOverItem(data.HoveredItem);
                if (data.IsPickingup)
                {
                    Debug.Log("Hit item.");
                    data.HoveredItem.RPC_Pickup(this);
                }
            }
            if (data.IsUsing)
            {
                data.HoveredItem.RPC_Use();
            }
            if (data.IsDropping)
            {
                data.HoveredItem.RPC_Drop();
            }
        }

        private void HandleHotbarControllerData(PlayerHotbarControllerData data)
        {
            InvokeChangeItemSelection(data.HotbarIndex);
        }
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