using System.Collections;
using System.Collections.Generic;
using CURSR.Settings;
using Fusion;
using UnityEngine;

namespace CURSR.Input
{
    public class InputPoller
    {
        private readonly InputActions _inputActions;

        // Constructor
        protected InputPoller()
        {
            _inputActions = new();
            Disable();
        }

        public void Enable() => _inputActions.Enable();
        public void Disable() => _inputActions.Disable();

        public PlayerInputStruct PollInput()
        {
            var playerInputStruct = new PlayerInputStruct();
            
            var wasdCache = _inputActions.Player.WASD.ReadValue<Vector2>();
            playerInputStruct.WASD = new Vector2(Mathf.Clamp((float)wasdCache.x, -1, 1), Mathf.Clamp((float)wasdCache.y, -1, 1));
            playerInputStruct.MouseDelta = _inputActions.Player.MouseDelta.ReadValue<Vector2>();
            playerInputStruct.LeftClick = _inputActions.Player.LeftClick.triggered;
            playerInputStruct.LeftClickHold = _inputActions.Player.LeftClick.IsPressed();
            playerInputStruct.RightClick = _inputActions.Player.RightClick.triggered;
            playerInputStruct.RightClickHold = _inputActions.Player.RightClick.IsPressed();
            playerInputStruct.Shift = _inputActions.Player.Shift.IsPressed();
            playerInputStruct.Ctrl = _inputActions.Player.Ctrl.IsPressed();
            playerInputStruct.Spacebar = _inputActions.Player.Spacebar.IsPressed();
            playerInputStruct.PauseEscape = _inputActions.Player.PauseEscape.triggered;
            playerInputStruct.Q = _inputActions.Player.Q.triggered;
            playerInputStruct.E = _inputActions.Player.E.triggered;
            playerInputStruct.F = _inputActions.Player.F.triggered;
            playerInputStruct.C = _inputActions.Player.C.triggered;
            playerInputStruct.R = _inputActions.Player.R.triggered;
            if (_inputActions.Player.NumKey.triggered)
                playerInputStruct.NumKey = (int)_inputActions.Player.NumKey.ReadValue<float>();
            playerInputStruct.Scroll = Mathf.Clamp((float)_inputActions.Player.Scroll.ReadValue<float>(), -1, 1);

            return playerInputStruct;
        }
    }
    
    public struct PlayerInputStruct
    {
        public Vector2 WASD;
        public Vector2 MouseDelta;
        public float Scroll;
        public int NumKey;
        public bool LeftClick, LeftClickHold, RightClick, RightClickHold, Shift, Ctrl, Spacebar, PauseEscape, Q, E, F, C, R;
    }
    public struct PlayerMovementInputStruct : INetworkInput
    {
        public static PlayerMovementInputStruct GetFromPlayerInputStruct(PlayerInputStruct playerInputStruct)
        {
            return new()
            {
                moveInput = playerInputStruct.WASD,
                isSprinting = playerInputStruct.Shift,
                isJumping = playerInputStruct.Spacebar,
                isCrouching = playerInputStruct.Ctrl,
                toggleFlying = playerInputStruct.F,
            };
        }
        public Vector2 moveInput { get; private set; }
        public bool isSprinting { get; private set; }
        public bool isJumping { get; private set; }
        public bool isCrouching { get; private set; }
        public bool toggleFlying { get; private set; }
    }
    public struct PlayerDisplayInputStruct
    {
        public static PlayerDisplayInputStruct GetFromPlayerInputStruct(PlayerInputStruct playerInputStruct)
        {
            return new()
            {
                TogglePauseOrEscape = playerInputStruct.PauseEscape,
                toggleBuildSection = playerInputStruct.Q,
                toggleSendMenu = playerInputStruct.E,
                toggleShopMenu = playerInputStruct.C,
                towerSelectNum = playerInputStruct.NumKey,
                towerSelectUp = playerInputStruct.Scroll > 0,
                towerSelectDown = playerInputStruct.Scroll < 0,
            };
        }
        public bool TogglePauseOrEscape { get; private set; }
        public bool toggleBuildSection { get; private set; }
        public bool toggleSendMenu { get; private set; }
        public bool toggleShopMenu { get; private set; }
        public int towerSelectNum { get; private set; }
        public bool towerSelectUp { get; private set; }
        public bool towerSelectDown { get; private set; }
    }
}
