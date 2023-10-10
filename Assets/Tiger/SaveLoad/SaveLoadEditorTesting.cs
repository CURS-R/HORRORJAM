using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CURSR
{
    public class SaveLoadEditorTesting : MonoBehaviour
    {
        [Header("Saving Game")]
        [SerializeField] private TextMeshProUGUI _username;
        [SerializeField] private List<string> _seeds;

        [Header("Loading Game")]
        [SerializeField] private TextMeshProUGUI _usernameLoad;
        [SerializeField] private TextMeshProUGUI _seedsLoad;

        public void SaveGameButtonPressed()
        {
            GameManager.Instance.PlayerAttributes.SaveGame(this._username.text, this._seeds);
        }

        public void LoadGameButtonPressed()
        {
            GameManager.Instance.LoadPlayerData();
            this._usernameLoad.text = $"Username: {GameManager.Instance.PlayerData.UserName}";

            this._seedsLoad.text = $"Seeds: ";
            for (int i = 0; i < this._seeds.Count; i++)
            {
                this._seedsLoad.text += this._seeds[i] + ", ";
            }
        }
    }
}
