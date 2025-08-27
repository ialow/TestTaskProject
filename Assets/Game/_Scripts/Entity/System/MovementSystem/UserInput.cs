using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [CreateAssetMenu(menuName = "Config/UserInput")]
    public class UserInput : ScriptableObject, DefaultInput.IGameplayActions, DefaultInput.IUIActions
    {
        private DefaultInput _input;

        public UserInput Enable()
        {
            if (_input is null)
            {
                _input = new DefaultInput();
                _input.Gameplay.SetCallbacks(this);
                _input.UI.SetCallbacks(this);
            }

            return this;
        }

        public void Disable()
        {
            _input.Gameplay.Disable();
            _input.UI.Disable();
        }

        public event Action<Vector2> MoveEvent;
        public event Action<Vector2> RotationEvent;

        public event Action PerformedUseItemEvent;
        public event Action CanceledUseItemEvent;

        public event Action<int> ToolbarEvent;
        public event Action ThrowItemEvent;

        public event Action PuaseEvent;
        public event Action ResumeEvent;
        public event Action Restart;

        public void OnGameplay()
        {
            _input.Gameplay.Enable();
            _input.UI.Disable();
        }

        public void OnUI()
        {
            _input.Gameplay.Disable();
            _input.UI.Enable();
        }

        public void Disposible()
        {
            Restart = null;
            ResumeEvent = null;
            PuaseEvent = null;
            ThrowItemEvent = null;
            ToolbarEvent = null;
            CanceledUseItemEvent = null;
            PerformedUseItemEvent = null;
            RotationEvent = null;
            MoveEvent = null;
        }

        public void OnRestart(InputAction.CallbackContext context)
        {
            Restart?.Invoke();
        }

        void DefaultInput.IGameplayActions.OnPause(InputAction.CallbackContext context)
        {
            PuaseEvent?.Invoke();
        }

        void DefaultInput.IUIActions.OnPause(InputAction.CallbackContext context)
        {
            ResumeEvent?.Invoke();
        }

        void DefaultInput.IGameplayActions.OnThrowItem(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Canceled)
                ThrowItemEvent?.Invoke();
        }

        void DefaultInput.IGameplayActions.OnToolbar(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Canceled)
            {
                var key = Convert.ToUInt16(context.control.name);
                ToolbarEvent?.Invoke(key);
            }
        }

        void DefaultInput.IGameplayActions.OnUseItem(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                PerformedUseItemEvent?.Invoke();

            else if (context.phase == InputActionPhase.Canceled)
                CanceledUseItemEvent?.Invoke();
        }

        void DefaultInput.IGameplayActions.OnWalk(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnMouse(InputAction.CallbackContext context)
        {
            RotationEvent?.Invoke(context.ReadValue<Vector2>());
        }
    }
}
