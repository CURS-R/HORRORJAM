using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using CURSR.Settings;

namespace CURSR.Game
{
    public class Enemy : NetworkBehaviour
    {
        public void Init(SettingsContainer settingsContainer)
        {
            _settingsContainer = settingsContainer;
        }
        private SettingsContainer _settingsContainer;

        public override void Spawned()
        {
            base.Spawned();
        }
        
        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority)
                return;
            // TODO:
        }
    }
}