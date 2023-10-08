using System;
using System.Collections;
using System.Collections.Generic;
using CURSR.Game;
using CURSR.Utils;
using Fusion;
using UnityEngine;

namespace CURSR.Network
{
    [CreateAssetMenu(fileName = "GameContainer", menuName = "CURSR/Container/Game")]
    public class GameContainer : ScriptableObject
    {
        // Serialized
        [field: Header("Prefabs")]
        [field:SerializeField] public Player PlayerPrefab { get; private set; }




    }
}