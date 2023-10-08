using CURSR.Utils;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CURSR.Settings
{
    [CreateAssetMenu(fileName = "NetworkSettings", menuName = "CURSR/Settings/Network")]
    public class NetworkSettings : ScriptableObject
    {
        [field: SerializeField] public SceneReference MultiplayerScene { get; private set; }
        [field: SerializeField] public GameMode GameMode { get; set; }
        [field: SerializeField] public string SessionName { get; set; }
        public StartGameArgs GetStartArgs(byte[] connectionToken) => new StartGameArgs()
        {
            GameMode = GameMode,
            SessionName = SessionName,
            Scene = SceneManager.GetSceneByPath(MultiplayerScene.ScenePath).buildIndex,
            ConnectionToken = connectionToken,
        };
    }
}