using Fusion;
using UnityEngine;
using CURSR.Input;
using CURSR.Settings;
using CURSR.Utils;

namespace CURSR.Game
{
    public class PlayerViewController : InputPoller
    {
        public PlayerViewController(PlayerViewSettings settings, Transform viewTransform)
        {
            _settings = settings;
            _viewTransform = viewTransform;
        }
        private readonly PlayerViewSettings _settings; // TODO: sensitivity setting
        private readonly Transform _viewTransform;
        
        // Externals
        private Angle yaw;
        public Angle GetYaw() => _viewTransform.rotation.eulerAngles.x;
        private Angle pitch;
        public Angle GetPitch() => _viewTransform.rotation.eulerAngles.y;

        public void Process()
        {
            var input = PollInput();
            DoRotateCamera(input.MouseDelta);
        }
        
        private void DoRotateCamera(Vector2 delta)
        {
            yaw += delta.x;
            pitch += delta.y;
            
            pitch = AngleUtil.CustomClampAngle(pitch);
            
            var rotation = Quaternion.Euler((float)pitch, (float)yaw, 0);
            _viewTransform.rotation = rotation;
        }
    }
}