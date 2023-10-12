using System;
using Fusion;
using UnityEngine;
using CURSR.Game;

namespace CURSR.Network
{
    public class Game : NetworkBehaviour
    {
        private GameContainer _gameContainer;
        
        public void Init(GameContainer gameContainer)
        {
            _gameContainer = gameContainer;
        }
        
        [Networked, Capacity(32), UnitySerializeField]
        public NetworkDictionary<int, Player> Players { get; }

        public override void Spawned()
        {
            base.Spawned();
        }

        private void FixedUpdate()
        {
            
        }
        
        private Player SpawnPlayer(int ownerID, PlayerRef inputAuth)
        {
            if (Players.ContainsKey(ownerID))
            {
                Debug.Log($"Game's SpawnPlayer() owner: {ownerID} but it already existed, returning that existing player.");
                var existingPlayer = Players.Get(ownerID);
                var existingNO = existingPlayer.gameObject.GetComponent<NetworkObject>();
                existingNO.AssignInputAuthority(inputAuth);
                Runner.SetPlayerObject(inputAuth, existingPlayer.GetBehaviour<NetworkObject>());
                existingPlayer.Spawned();
                return existingPlayer;
            }
            var prefab = _gameContainer.PlayerPrefab;
            // TODO: spawnpoints and rotations
            var spawnPos = Vector3.zero;
            var spawnRot = Quaternion.identity;
            var newPlayer = Runner.Spawn(prefab, position: spawnPos, rotation: spawnRot, inputAuthority: inputAuth, onBeforeSpawned: (runner, no) =>
            {
                var player = no.GetBehaviour<Player>();
                // TODO: Init Player
                //player.Init();
                Runner.SetPlayerObject(inputAuth, player.GetBehaviour<NetworkObject>());
            });
            Players.Set(ownerID, newPlayer);
            return newPlayer;
        }
    }
}