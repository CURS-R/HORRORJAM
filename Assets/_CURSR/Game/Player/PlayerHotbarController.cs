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
            get => Mathf.Clamp(_hotbarIndex, 0, _settings.MaxHotbarItems);
            set => _hotbarIndex = Mathf.Clamp(value, 0, _settings.MaxHotbarItems);
        }
        
        public PlayerHotbarControllerData Process(float deltaTime)
        {
            var data = new PlayerHotbarControllerData();
            var input = PollInput();
            
            bool up = input.Scroll > 0;
            bool down = input.Scroll < 1;
            if (up)
            {
                hotbarIndex++;
            }
            if (down)
            {
                hotbarIndex--;
            }
            data.HotbarIndex = hotbarIndex;

            return data;
        }
    }
    
    public class PlayerHotbarControllerData
    {
        public int HotbarIndex;
    }
}