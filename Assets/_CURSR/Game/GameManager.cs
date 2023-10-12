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

        private void StartGame()
        {
        }

        private void PlayerJoinGame(int id, PlayerRef playerRef)
        {
            //var newPlayer = gameContainer.Game.SpawnPlayer(id, playerRef);
        }
        
        private Game SpawnGame(NetworkRunner runner, int ownerID, PlayerRef inputAuth)
        {
            var prefab = gameContainer.GamePrefab;
            var newGame = runner.Spawn(prefab, inputAuthority: inputAuth, onBeforeSpawned: (runner, no) =>
            {
                var game = no.GetBehaviour<Game>();
                // TODO: Init Game
                // game.Init();
            });
            Log("Game has been spawned.");
            return newGame;
        }

        private void PlayerToggleLocalRepresentation(int id, bool on)
        {
            if (gameContainer.Game.Players.TryGet(id, out var player))
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