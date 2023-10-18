using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CURSR
{
    public class PlayerSaveAttributes : MonoBehaviour
    {
        public string UserName;
        public List<string> Seeds;

        /// <summary>
        /// Load attributes based on Game Manger's player data instance.
        /// </summary>
        public void LoadAttributes()
        {
            this.UserName = GameManager.Instance.PlayerData.UserName;
            this.Seeds = GameManager.Instance.PlayerData.Seeds.ToList<string>();

            Debug.Log("<color=green>Attributes Loaded.</color>");
        }

        /// <summary>
        /// Loads attributes based on passed in player data.
        /// </summary>
        /// <param name="playerData"></param>
        public void LoadAttributes(ref PlayerData playerData)
        {
            this.UserName = playerData.UserName;
            this.Seeds = playerData.Seeds?.ToList<string>();

            Debug.Log("<color=green>Attributes Loaded.</color>");
        }

        #region FOR PROOF OF CONCEPT
        public void SaveUserName(string username)
        {
            this.UserName = username;
            this.SaveGame();
        }

        public void SaveGame(string username, List<string> seeds)
        {
            this.UserName = username;
            this.Seeds = seeds;
            this.SaveGame();
        }
        #endregion

        /// <summary>
        /// Functionality for saving the game.
        /// </summary>
        public void SaveGame()
        {
            SaveLoadSystem.SavePlayerData(this);
            Debug.Log($"Attributes saved. Username: {this.UserName}");
        }
    }
}
