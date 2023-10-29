using CURSR.Input;
using CURSR.Settings;
using CURSR.Utils;
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
        
        public PlayerInteractionControllerData Process(float deltaTime)
        {
            var data = new PlayerInteractionControllerData();
            var input = PollInput();

            if (DrawRayForItem(out data.HoveredItem))
            {
                if (input.LeftClick)
                {
                    data.IsPickingup = true;
                }
            }

            return data;
        }

        private bool DrawRayForItem(out Item item)
        {
            item = null;
            Ray ray = new(_viewTransform.position, _viewTransform.forward);

            if (Physics.Raycast(ray, out var hit, _settings.RayDistance, _settings.HitLayers))
            {
                item = Colliders.GetComponentUpwards<Item>(hit.collider.gameObject);
                if (item != null)
                {
                    return true;
                }
            }

            Debug.DrawRay(_viewTransform.position, _viewTransform.forward * _settings.RayDistance, Color.red);

            return false;
        }
    }
    
    public class PlayerInteractionControllerData
    {
        public Item HoveredItem = null;
        public bool IsHovering => HoveredItem != null;
        public bool IsPickingup = false;
    }
}