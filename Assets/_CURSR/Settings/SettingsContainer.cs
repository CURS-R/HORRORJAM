using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CURSR.Settings
{
    [CreateAssetMenu(fileName = "SettingsContainer", menuName = "CURSR/Container/Settings")]
    public class SettingsContainer : ScriptableObject
    {
        [field: SerializeField] public PlayerSettings PlayerSettings { get; private set; }
        
        
    }
}
