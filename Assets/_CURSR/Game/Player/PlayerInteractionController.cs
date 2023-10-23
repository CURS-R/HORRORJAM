using CURSR.Input;
using CURSR.Settings;
using UnityEngine;

namespace CURSR.Game
{
    public class PlayerInteractionController : InputPoller
    {
        public PlayerInteractionController(PlayerInteractionSettings settings, Transform viewTransform)
        {
            _settings = settings;
            _viewTransform = viewTransform;
        }
        private readonly PlayerInteractionSettings _settings;
        private readonly Transform _viewTransform;
        
        public void Process(float deltaTime)
        {
            var input = PollInput();

            if (input.LeftClick)
            {
                if (DrawRayForItem(out var item))
                {
                    Debug.Log("Hit item.");
                    // TODO: interact with item
                }
            }
            if (input.RightClick)
            {
                // TODO: use item
            }
            if (input.Q)
            {
                // TODO: drop item
            }
        }

        private bool DrawRayForItem(out Item item)
        {
            item = null;
            Ray ray = new(_viewTransform.position, _viewTransform.forward);

            if (Physics.Raycast(ray, out var hit, _settings.rayDistance, _settings.hitLayers))
            {
                Debug.Log("Hit object: " + hit.collider.name);
                item = Utils.Colliders.GetComponentUpwards<Item>(hit.collider.gameObject);
        
                if (item != null)
                {
                    return true;
                }
            }

            Debug.DrawRay(_viewTransform.position, _viewTransform.forward * _settings.rayDistance, Color.red);

            return false;
        }
    }
}