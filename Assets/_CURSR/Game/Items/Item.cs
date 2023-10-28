using System;
using System.Collections;
using System.Collections.Generic;
using CURSR.Settings;
using CURSR.Utils;
using Fusion;
using JetBrains.Annotations;
using UnityEngine;

namespace CURSR.Game
{
    [RequireComponent(typeof(NetworkRigidbody))]
    public class Item : NetworkBehaviour
    {
        private NetworkRigidbody rb => GetComponent<NetworkRigidbody>();
        
        [field:SerializeField] private GameContainer gameContainer;
        
        [field:SerializeField] private Transform visualTransform;

        [Networked]
        [HideInInspector] public int Index { get; set; }
        [Networked]
        [HideInInspector] public Player Holder { get; set; }

        public ItemSO ItemSO => gameContainer.ItemSOsRegistry.Items[Index];

        [CanBeNull] private GameObject visualGameObject;
        [CanBeNull] private List<Collider> colliders => Utils.Colliders.GetAllColliders(rb.gameObject);

        public override void Spawned()
        {
            base.Spawned();
            if (ItemSO.visualGameObject != null)
                visualGameObject = Instantiate(ItemSO.visualGameObject, visualTransform);
            else
                Debug.Log("Didn't have a visualGameObject", this);
        }
        
        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority)
                return;
            // TODO: ?
        }

        private void LateUpdate()
        {
            if (Holder == null)
            {
                // TODO: ?
            }
        }

        public void SetActive(bool active)
        {
            if (active)
            {
                if (visualGameObject != null)
                    visualGameObject.SetActive(true);
                if (colliders != null)
                    foreach (var collider in colliders)
                        collider.enabled = true;
                rb.Rigidbody.isKinematic = false;
            }
            else
            {
                if (visualGameObject != null)
                    visualGameObject.SetActive(false);
                if (colliders != null)
                    foreach (var collider in colliders)
                        collider.enabled = false;
                rb.Rigidbody.isKinematic = true;
            }
        }

        public void Teleport(Transform transform) => Teleport(transform.position, transform.rotation);
        public void Teleport(Vector3 position, Quaternion rotation = default)
        {
            if (rotation == default)
                rotation = Quaternion.identity;

            rb.transform.SetPositionAndRotation(position, rotation);
        }

        [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
        public void RPC_Pickup(Player player)
        {
            SetActive(false);
            if (HasStateAuthority)
            {
                Holder = player;
                Holder.Items.Add(this);
            }
        }
        
        [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
        public virtual void RPC_Use()
        {
            // TODO: Item.RPC_Use
        }
        
        [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
        public void RPC_Drop()
        {
            SetActive(true);
            if (HasStateAuthority)
            {
                if (Holder == null)
                {
                    Debug.Log("Holder was null");
                    return;
                }
                Debug.Log(Holder.localViewTransform);
                var newPos = Holder.localViewTransform;
                Teleport(newPos);
                float force = 5f; // LATER: difference drop-forces
                rb.Rigidbody.AddForce(newPos.forward * force, ForceMode.Impulse);
                Holder.Items.Remove(this);
                Holder = default;
            }
        }
    }
}
