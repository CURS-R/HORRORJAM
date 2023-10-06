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

        public int Currency;
        public string Seed;
        public float[] PlayerPos;

        public PlayerData(PlayerSaveAttributes saveAttributes)
        {
            this.Currency = saveAttributes.Currency;
            this.Seed = saveAttributes.Seed;
            this.PlayerPos = saveAttributes.PlayerPos;
        }
    }
}
