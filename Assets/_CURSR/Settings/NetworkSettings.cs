using CURSR.Utils;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CURSR.Settings
{
    [CreateAssetMenu(fileName = "NetworkSettingsContainer", menuName = "CURSR/Settings/Network")]
    public class NetworkSettingsContainer : ScriptableObject
    {
        [field: SerializeField] public SceneReference MultiplayerScene { get; private set; }
        [field: SerializeField] public GameMode GameMode { get; set; } // TODO: modifying this with UI?
        [field: SerializeField] public string SessionName { get; set; } // LATER: modifying this with UI
        public StartGameArgs GetStartArgs(byte[] connectionToken) => new StartGameArgs()
        {
            GameMode = GameMode,
            SessionName = SessionName,
            Scene = SceneManager.GetSceneByPath(MultiplayerScene.ScenePath).buildIndex,
            ConnectionToken = connectionToken,
        };
    }
}