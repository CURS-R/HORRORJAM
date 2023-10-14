using System;
using Fusion;
using UnityEngine;
using CURSR.Game;

namespace CURSR.Network
{
    public class Game : NetworkBehaviour
    {
        [field:SerializeField] private GameContainer gameContainer;

        [Networked] 
        public GameData Data { get; set; }
        [Networked, Capacity(32), UnitySerializeField]
        public NetworkDictionary<PlayerRef, Player> Players { get; }

        public override void Spawned()
        {
            if (gameContainer.Game == null)
                gameContainer.Game = this;
            else
                Debug.Log("Game in gameContainer wasn't null? There's a large problem unfolding.");
        }

        private void FixedUpdate()
        {
            
        }
    }

    public struct GameData : INetworkStruct
    {
        public GAMESTATE State;
    }
    
    public enum GAMESTATE
    {
        INIT,
        LOBBY,
        PLAYING,
        OVER,
    }
}