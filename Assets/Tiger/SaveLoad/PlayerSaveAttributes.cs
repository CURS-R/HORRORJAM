using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CURSR
{
    public class PlayerSaveAttributes : MonoBehaviour
    {
        public int Currency;
        public string Seed;
        public float[] PlayerPos;

        public void LoadAttributes()
        {
            this.Currency = GameManager.Instance.PlayerData.Currency;
            this.Seed = GameManager.Instance.PlayerData.Seed;
            this.PlayerPos = GameManager.Instance.PlayerData.PlayerPos;

            Debug.Log("<color=green>Attributes Loaded.</color>");
        }

        public void SaveCurrency(int currency)
        {
            this.Currency = currency;
            this.SaveGame();
        }

        public void SaveSeed(string seed)
        {
            this.Seed = seed;
            this.SaveGame();
        }

        public void SaveGame()
        {
            SaveLoadSystem.SavePlayerData(this);
            Debug.Log($"Attributes saved.\nCurrency: {this.Currency} \nPlayerPos: {this.PlayerPos}");
        }
    }
}
