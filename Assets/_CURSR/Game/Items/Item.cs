using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace CURSR.Game
{
    public class Item : NetworkBehaviour
    {
        [field:SerializeField] private GameContainer gameContainer;
        
        [field:SerializeField] private Transform visualTransform;

        [Networked] public int Index { get; set; }
        [Networked] public Player Holder { get; set; }

        private ItemSO ItemSO => gameContainer.ItemsRegistry.Items[Index];

        public override void Spawned()
        {
            base.Spawned();
            if (ItemSO.visualGameObject != null)
                Instantiate(ItemSO.visualGameObject, visualTransform);
            else
                Debug.Log("Didn't have a visualGameObject", this);
        }
        
        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority)
                return;
            // TODO:
        }

        private void LateUpdate()
        {
            if (Holder == null)
            {
                // TODO:
            }
        }

        [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
        public void RPC_Pickup(Player player)
        {
            // TODO:
            player.Inventory.Add(Index);
            Holder = player;
        }
        
        [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
        public void RPC_Drop()
        {
            // TODO:
            Holder.Inventory.Add(Index);
            Holder = default;
        }
    }
}
