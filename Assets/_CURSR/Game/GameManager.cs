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
        
        private bool isHost(NetworkRunner runner) => runner.IsSinglePlayer || runner.IsServer;
        
        protected override void Init()
        {
            
        }

        private void OnEnable()
        {
            // TODO: subscribe to CreateRoomEvent for pre-game mechanics (lobby) 
            gameContainer.NetworkContainer.JoinRoomEvent += SpawnOrGetGame;

            // TODO: subscribe to PlayerLeaveRoomEvent to change game mechanics based on a player leaving (keep in mind rejoins)
            gameContainer.NetworkContainer.PlayerJoinRoomEvent += SpawnOrGetPlayer;
        }
        
        private void OnDisable()
        {
            // TODO: event unregisters
        }
        
        private void SpawnOrGetGame(NetworkRunner runner)
        {
            if (isHost(runner))
            {
                var prefab = gameContainer.GamePrefab;
                var newGame = runner.Spawn(
                    prefab:prefab, 
                    onBeforeSpawned:(runner, no) =>
                {
                    var game = no.GetBehaviour<Game>();
                    game.Init(gameContainer);
                });
                Log("Game has been spawned.");
            }

            gameContainer.Game = FindObjectOfType<Game>();
            Log("Game has been gotten.");
        }

        private void SpawnOrGetPlayer(NetworkRunner runner, PlayerRef playerRef)
        {
            if (isHost(runner))
            {
                var prefab = gameContainer.PlayerPrefab;
                var newPlayer = runner.Spawn(
                    prefab:prefab, 
                    inputAuthority:playerRef, 
                    onBeforeSpawned:(runner, no) =>
                {
                    var player = no.GetBehaviour<Player>();
                    player.Init(gameContainer.SettingsContainer);
                });
                Log("Player has been spawned.");
            }

            // TODO: getting new player locally?
            Log("Player has been gotten.");
        }

        private void PlayerToggleLocalRepresentation(int id, bool on)
        {
            if (gameContainer.Game.Players.TryGet(id, out var player))
                player.ToggleLocalRepresentation(on);
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