using UnityEngine;
using CURSR.Utils;

namespace CURSR.Saves
{
    public class SavesManager : Singleton<SavesManager>
    {
        [field:SerializeField] private SavesContainer savesContainer;

        protected override void Init()
        {
            savesContainer.PlayerData.Save();
            savesContainer.PlayerData.Load();
            var playerData = savesContainer.PlayerData;
        }
    }
}