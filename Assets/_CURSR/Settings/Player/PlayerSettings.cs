using UnityEngine;
using UnityEngine.Serialization;

namespace CURSR.Settings
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "CURSR/Settings/Player")]
    public class PlayerSettings : ScriptableObject
    {
        public PlayerMovementSettings PlayerMovementSettings;
        public PlayerViewSettings PlayerViewSettings;
        public PlayerInteractionSettings PlayerInteractionSettings;
        public PlayerInventorySettings PlayerInventorySettings;
    }
    
    [System.Serializable]
    public class PlayerMovementSettings
    {
        [field:SerializeField] public float WalkSpeed { get; private set; }
        [field:SerializeField] public float RunSpeed { get; private set; }
        [field:SerializeField] public float JumpSpeed { get; private set; }
        [field:SerializeField] public float FlySpeed { get; private set; }
        [field:SerializeField] public float FastFlySpeed { get; private set; }
        public float GetSpeed(bool forSprint, bool forFlying)
        {
            if (forSprint)
                return forFlying ? FastFlySpeed : RunSpeed;
            return forFlying ? FlySpeed : WalkSpeed;
        }
    }
    
    [System.Serializable]
    public class PlayerViewSettings
    {
        [field:SerializeField] public float Sensitivity { get; private set; }
    }

    [System.Serializable]
    public class PlayerInteractionSettings
    {
        [field:SerializeField] public float rayDistance { get; private set; }
        [field:SerializeField] public LayerMask hitLayers { get; private set; }
    }

    [System.Serializable]
    public class PlayerInventorySettings
    {
        [field:SerializeField] public int MaxInventoryHotbar { get; private set; }
    }
}