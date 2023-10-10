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
        
        protected override void Init()
        {
            
        }

        private void OnEnable()
        {
            // TODO: event registers
        }
        private void OnDisable()
        {
            // TODO: event unregisters
        }

        private void StartGame(NetworkRunner runner)
        {
            gameContainer.Game = 
                runner.Spawn(
                        prefab: gameContainer.GamePrefab, 
                        position: Vector3.zero, 
                        rotation: Quaternion.identity, 
                        inputAuthority: runner.LocalPlayer)
                    .GetComponent<Game>();
            Log("LiveGame has been spawned.");
        }

        private void PlayerJoinGame(int id, PlayerRef playerRef)
        {
            //var newPlayer = gameContainer.Game.SpawnPlayer(id, playerRef);
        }
        // TODO: Lucas pasted this from an earlier game
        /*
        private Player SpawnPlayer(int ownerID, PlayerRef inputAuth)
        {
            if (GamePlayers.ContainsKey(ownerID))
            {
                Log($"LiveGame SpawnPlayer() owner: {ownerID} but it already existed, returning that existing player.");
                var existingPlayerGB = GamePlayers.Get(ownerID);
                var existingNO = existingPlayerGB.gameObject.GetComponent<NetworkObject>();
                existingNO.AssignInputAuthority(inputAuth);
                Runner.SetPlayerObject(inputAuth, existingPlayerGB.GetBehaviour<NetworkObject>());
                existingPlayerGB.Spawned();
                return existingPlayerGB;
            }
            var prefab = gameContainer.playerGBPrefab;
            var spawnPos = gameContainer.mapGSO.Config.playerspawnpoint.position;
            var spawnRot = gameContainer.mapGSO.Config.playerspawnpoint.rotation;
            var newPlayerGB = Runner.Spawn(prefab, position: spawnPos, rotation: spawnRot, inputAuthority: inputAuth, onBeforeSpawned: (runner, no) =>
            {
                var GB = no.GetBehaviour<PlayerGB>();
                GB.Init(gameContainer.playerGSO.Config.Index, ownerID, true);
                Runner.SetPlayerObject(inputAuth, GB.GetBehaviour<NetworkObject>());
            });
            GamePlayers.Set(ownerID, newPlayerGB);
            return newPlayerGB;
        }
        */

        private void PlayerToggleLocalRepresentation(int id, bool on)
        {
            if (gameContainer.Game.GamePlayers.TryGet(id, out var player))
                player.ToggleLocalRepresentation(on);
        }

        private void PlayerLeaveLiveGame(int id)
        {
            // LATER: PlayerLeaveLiveGame
        }
        
        private void Log(string message) => Debug.Log(message, this);
    }

    public enum GAMESTATE
    {
        INIT,
        LOBBY,
        PLAYING,
        OVER,
    }
}