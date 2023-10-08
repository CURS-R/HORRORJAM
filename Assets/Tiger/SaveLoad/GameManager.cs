using CURSR.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CURSR
{
    public class GameManager : Utils.Singleton<GameManager>
    {
        public static GameManager Instance;
        public PlayerSaveAttributes PlayerAttributes;
        public PlayerData PlayerData;

        protected override void Init()
        {
            this.PlayerAttributes = GetComponent<PlayerSaveAttributes>();
            this.LoadPlayerData();

            this.PlayerAttributes.LoadAttributes();
        }

        /// <summary>
        /// Loads player data.
        /// </summary>
        public void LoadPlayerData()
        {
            this.PlayerData = SaveLoadSystem.LoadPlayerData();
            if (PlayerData != null)
            {
                Debug.Log("<color=green>Player Data Loaded.</color>");
            }
        }

        /// <summary>
        /// Returns if load file exist.
        /// </summary>
        /// <returns></returns>
        public bool LoadExists()
        {
            return SaveLoadSystem.LoadPlayerData() != null;
        }
    }
}
