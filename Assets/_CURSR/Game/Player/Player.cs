using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using CURSR.Settings;

namespace CURSR.Game
{
    public class Player : NetworkBehaviour
    {
        public void Init(SettingsContainer settingsContainer)
        {
            _settingsContainer = settingsContainer;
        }
        private SettingsContainer _settingsContainer;
        
        // Networked
        [Networked] public ref PlayerData PlayerData => ref MakeRef<PlayerData>();
        
        [field:SerializeField] public CharacterController localCC { get; private set; }
        [field:SerializeField] public Transform localViewTransform { get; private set; }
        
        private PlayerSettings _playerSettings => _settingsContainer.PlayerSettings;
        private PlayerCharacterController _playerCharacterController;
        private PlayerViewController _playerViewController;

        public override void Spawned()
        {
            // TODO: Refactor this?
            _playerCharacterController ??= new (_settingsContainer, localCC);
            _playerViewController ??= new (_settingsContainer, localViewTransform);
            ToggleLocalRepresentation(HasInputAuthority);
            base.Spawned();
        }
        
        /// <summary>
        /// This can cause de-sync if used improperly.
        /// </summary>
        public void ToggleLocalRepresentation(bool on)
        {
            if (on)
            {
                localCC.enabled = true;
                _playerCharacterController.Enable();
                _playerViewController.Enable();
            }
            else
            {
                localCC.enabled = false;
                _playerCharacterController.Disable();
                _playerViewController.Disable();
            }
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            if (!HasInputAuthority)
                return;
            var outgoing = new PlayerSyncStruct{
                ccPosition = localCC.transform.position,
                ccRotation = localCC.transform.rotation,
                viewRotation = localViewTransform.rotation,
            };
            input.Set(outgoing);
        }
        
        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority)
                return;
            if (GetInput(out PlayerSyncStruct incoming))
            {
                PlayerData.networkedCCPosition = incoming.ccPosition;
                PlayerData.networkedCCRotation = incoming.ccRotation;
                PlayerData.networkedViewPoint = incoming.viewRotation;
            }
        }

        public void Update()
        {
            if (HasInputAuthority)
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    _playerViewController.Process();
                    _playerCharacterController.ProcessRotate(_playerViewController.GetPitch(), Time.deltaTime);
                }
                _playerCharacterController.ProcessMove(Time.deltaTime);
            }
        }
    }
    
    public struct PlayerData : INetworkStruct
    {
        public int ownerID { get; set; }
        public Vector2Int index { get; set; }
        // Visual/Movement
        public Vector3 networkedCCPosition { get; set; }
        public Quaternion networkedCCRotation { get; set; }
        public Quaternion networkedViewPoint { get; set; }
    }

    // TODO: i think this should be changed ? it's weird
    public struct PlayerSyncStruct : INetworkInput
    {
        public Vector3 ccPosition { get; set; }
        public Quaternion ccRotation { get; set; }
        public Quaternion viewRotation { get; set; }
    }
}