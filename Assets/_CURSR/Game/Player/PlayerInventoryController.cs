using CURSR.Input;

namespace CURSR.Game
{
    public class PlayerInventoryController : InputPoller
    {
        public void Process(float deltaTime)
        {
            var input = PollInput();
            bool up = input.Scroll > 0;
            bool down = input.Scroll < 1;
            if (up)
            {
                // TODO: scroll up in inventory
            }
            if (down)
            {
                // TODO: scroll down in inventory
            }
        }
    }
}