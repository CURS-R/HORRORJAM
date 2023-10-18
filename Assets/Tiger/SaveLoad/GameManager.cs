using CURSR.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CURSR
{
    public class GameManager : Utils.Singleton<GameManager>
    {
        public PlayerSaveAttributes PlayerAttributes;
        public PlayerData PlayerData;

        protected override void Init()
        {
            this.PlayerAttributes = GetComponent<PlayerSaveAttributes>();

            this.LoadPlayerData();
            if (this.PlayerData != null)
            {
                this.PlayerAttributes.LoadAttributes(ref this.PlayerData);
            }
        }

        /// <summary>
        /// Loads player data.
        /// </summary>
        public void LoadPlayerData()
        {
            if (this.IsGameRunningOnWindows() || this.IsGameRunningInEditor())
            {
                this.PlayerData = SaveLoadSystem.LoadPlayerData();
                if (PlayerData != null)
                {
                    Debug.Log("<color=green>Player Data Loaded.</color>");
                }
            }
            else if(this.IsGameRunningOnWeb())
            {

            }
        }

        /// <summary>
        /// Returns if load file exist.
        /// </summary>
        /// <returns></returns>
        public bool LoadExists()
        {
            if (this.IsGameRunningOnWindows())
            {
                return SaveLoadSystem.LoadPlayerData() != null;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Return if game is running on windows.
        /// </summary>
        /// <returns></returns>
        public bool IsGameRunningOnWindows()
        {
            return Application.platform == RuntimePlatform.WindowsPlayer;
        }

        /// <summary>
        /// Return if game is running on Web.
        /// </summary>
        /// <returns></returns>
        public bool IsGameRunningOnWeb()
        {
            return Application.platform == RuntimePlatform.WebGLPlayer;
        }

        /// <summary>
        /// Returns if game is running in editor.
        /// </summary>
        /// <returns></returns>
        public bool IsGameRunningInEditor()
        {
            return Application.isEditor;
        }
    }
}
