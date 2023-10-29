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
        
        private int _hotbarIndex = 0;
        private int hotbarIndex
        {
            get => (int)Mathf.Repeat(_hotbarIndex, _settings.MaxCapacity);
            set => _hotbarIndex = (int)Mathf.Repeat(value, _settings.MaxCapacity);
        }
        
        public PlayerHotbarControllerData Process(float deltaTime)
        {
            var data = new PlayerHotbarControllerData();
            var input = PollInput();

            if (input.Scroll < 0)
            {
                hotbarIndex++;
            }
            else if (input.Scroll > 0)
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