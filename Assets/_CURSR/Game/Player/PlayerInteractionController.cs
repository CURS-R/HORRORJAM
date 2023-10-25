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
        
        public InteractionControllerData Process(float deltaTime)
        {
            var input = PollInput();
            var data = new InteractionControllerData();

            if (DrawRayForItem(out var item))
            {
                data.hoveredItem = item;
                if (input.LeftClick)
                {
                    data.isPickingup = true;
                    Debug.Log("Hit item.");
                    // TODO: interact with item

                }
            }
            if (input.RightClick)
            {
                data.isUsing = true;
                // TODO: use item
            }
            if (input.Q)
            {
                data.isDropping = true;
                // TODO: drop item
            }

            return data;
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
    
    public class InteractionControllerData
    {
        public Item hoveredItem = null;
        public bool isHovering = false;
        public bool isPickingup = false;
        public bool isUsing = false;
        public bool isDropping = false;
    }
}