using System;
using System.Collections;
using System.Collections.Generic;
using CURSR.Game;
using CURSR.Network;
using CURSR.Utils;
using UnityEngine;

namespace CURSR.Camera
{
    public class CameraManager : Singleton<CameraManager>
    {
        [field:SerializeField] private NetworkContainer networkContainer;
        [field:SerializeField] private GameContainer gameContainer;
        
        [HideInInspector] public Camera Camera;
        
        protected override void Init()
        {
            
        }

        private void OnEnable()
        {
            gameContainer.LocalPlayerSpawnedEvent += LockOnToPlayer;
        }
        private void OnDisable()
        {
            // TODO: event unregisters
        }
        
        private void LockOnToPlayer(Player player)
        {
            Camera.GiveTarget(player.localViewTransform);
        }
    }
}
