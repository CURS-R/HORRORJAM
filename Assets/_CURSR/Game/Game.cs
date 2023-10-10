using System;
using Fusion;
using UnityEngine;
using CURSR.Game;

namespace CURSR.Network
{
    public class Game : NetworkBehaviour
    {
        [Networked, Capacity(32), UnitySerializeField]
        public NetworkDictionary<int, Player> GamePlayers => default;

        public override void Spawned()
        {
            base.Spawned();
        }

        private void FixedUpdate()
        {
            
        }
    }
}