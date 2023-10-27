using CURSR.Input;
using CURSR.Settings;
using UnityEngine;

namespace CURSR.Game
{
    public class PlayerHotbarController : InputPoller
    {
        public PlayerHotbarController(PlayerHotbarSettings settings)
        {
            _settings = settings;
        }
        private readonly PlayerHotbarSettings _settings;
        
        private int _hotbarIndex;
        private int hotbarIndex
        {
            get => Mathf.Clamp(_hotbarIndex, 0, _settings.MaxCapacity);
            set => _hotbarIndex = Mathf.Clamp(value, 0, _settings.MaxCapacity);
        }
        
        public PlayerHotbarControllerData Process(float deltaTime)
        {
            var data = new PlayerHotbarControllerData();
            var input = PollInput();
            
            if (input.Scroll > 0)
            {
                hotbarIndex++;
            }
            if (input.Scroll < 1)
            {
                hotbarIndex--;
            }
            data.HotbarIndex = hotbarIndex;
            if (input.RightClick)
            {
                data.IsUsing = true;
            }
            if (input.Q)
            {
                data.IsDropping = true;
            }

            return data;
        }
    }
    
    public class PlayerHotbarControllerData
    {
        public int HotbarIndex;
        public bool IsUsing = false;
        public bool IsDropping = false;
    }
}