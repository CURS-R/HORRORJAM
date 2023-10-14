using UnityEngine;
using Fusion;
using CURSR.Game;
using CURSR.Input;
using CURSR.Network;
using CURSR.Saves;
using CURSR.Settings;
using CURSR.Utils;

namespace CURSR.Network
{
    public class GameManager : Singleton<GameManager>
    {
        [field:SerializeField] private GameContainer gameContainer;
        [field:SerializeField] private NetworkContainer networkContainer;
        
        private bool isHost(NetworkRunner runner) => runner.IsSinglePlayer || runner.IsServer;
        
        protected override void Init()
        {
            
        }

        private void OnEnable()
        {
            networkContainer.LocalJoinRoomEvent += SpawnGame;
            networkContainer.PlayerJoinRoomEvent += SpawnPlayer;
        }
        
        private void OnDisable()
        {
            // TODO: event unregisters
        }
        
        private void SpawnGame(NetworkRunner runner)
        {
            if (!isHost(runner)) return;
            
            var prefab = gameContainer.GamePrefab;
            var newGame = runner.Spawn(
                prefab:prefab, 
                onBeforeSpawned:(runner, no) =>
                {
                    var game = no.GetBehaviour<Game>();
                });
            Log("Game has been spawned.");
        }

        private void SpawnPlayer(NetworkRunner runner, PlayerRef playerRef)
        {
            if (!isHost(runner)) return;

            // Spawning or handling rejoin of the player object
            if (gameContainer.Game.Players.TryGet(playerRef, out var player))
            {
                Debug.Log($"(gameContainer.Game.Players.TryGet({playerRef.PlayerId}), found, returning that existing player.");
                player.gameObject.GetComponent<NetworkObject>().AssignInputAuthority(playerRef);
                player.Spawned();
            }
            else
            {
                // TODO: spawnpoints and rotations
                var spawnPos = Vector3.zero;
                var spawnRot = Quaternion.identity;
                player = runner.Spawn(
                    prefab:gameContainer.PlayerPrefab, 
                    inputAuthority:playerRef,
                    position: spawnPos,
                    rotation: spawnRot,
                    onBeforeSpawned:(runner, no) =>
                    {
                        var player = no.GetBehaviour<Player>();
                        gameContainer.Game.Players.Add(playerRef, player);
                    });
                Log("We spawned a new player.");
            }
            runner.SetPlayerObject(playerRef, player.GetBehaviour<NetworkObject>());
            gameContainer.Game.Players.Set(playerRef, player);
        }
        
        private void Log(string message) => Debug.Log(message, this);
    }
}