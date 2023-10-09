using UnityEngine;
using CURSR.Utils;

namespace CURSR.Saves
{
    [CreateAssetMenu(fileName = "SavesContainer", menuName = "CURSR/Container/Saves")]
    public class SavesContainer : ScriptableObject
    {
        public PlayerSaveData PlayerData;
    }
}