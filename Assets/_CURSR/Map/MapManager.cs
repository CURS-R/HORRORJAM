using UnityEngine;
using Fusion;
using CURSR.Input;
using CURSR.Saves;
using CURSR.Settings;
using CURSR.Utils;

namespace CURSR.Network
{
    public class MapManager : Singleton<MapManager>
    {
        [field:SerializeField] private MapContainer mapContainer;
        
        protected override void Init()
        {
            
        }

        private void OnEnable()
        {
            // TODO: event registers
        }
        private void OnDisable()
        {
            // TODO: event unregisters
        }


        
        private void Log(string message) => Debug.Log(message, this);
    }
}