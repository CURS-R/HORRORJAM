using UnityEngine;
using CURSR.Utils;

namespace CURSR.Saves
{
    public class SavesManager : Singleton<SavesManager>
    {
        [field:SerializeField] private SavesContainer savesContainer;

        protected override void Init()
        {
            savesContainer.PlayerSaveData.Save();
            savesContainer.PlayerSaveData.Load();
            var playerData = savesContainer.PlayerSaveData;
        }
    }
}