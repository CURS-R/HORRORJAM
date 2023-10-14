using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CURSR
{
    [System.Serializable]
    public class PlayerData
    {
        // Only store basic data types
        // string
        // bool
        // float
        // int[]

        public string UserName;
        public string[] Seeds;

        public PlayerData(PlayerSaveAttributes saveAttributes)
        {
            this.UserName = saveAttributes.UserName;
            this.Seeds = saveAttributes.Seeds.ToArray();
        }
    }
}