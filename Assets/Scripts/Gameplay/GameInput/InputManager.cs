using Core;
using UnityEngine;

namespace Gameplay.GameInput
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private LayerMask interactableMask;
        [SerializeField] private float rayCastDistance;

        public static InputManager Instance;

        private bool _inputEnabled = true;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (!_inputEnabled)
                return;

            if (GameManager.Instance.CurrentState
                != GameState.WaitingInput)
                return;

#if UNITY_EDITOR
            HandleMouseInput();
#else
            HandleTouchInput();
#endif
        }

        private void HandleTouchInput()
        {
            if(Input.touchCount <= 0)
                return;

            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                TrySelect(touch.position);
            }
        }

        private void HandleMouseInput()
        {
            if(Input.GetMouseButtonDown(0))
            {
                TrySelect(Input.mousePosition);
            }
        }

        public void HandleInput()
        {
            if (GameManager.Instance.CurrentState
                != GameState.WaitingInput)
            {
                return;
            }
        }

        public void TrySelect(Vector2 screenPos)
        {
            var ray = mainCamera.ScreenPointToRay(screenPos);

            if(Physics.Raycast(
                   ray,
                   out RaycastHit hit,
                   rayCastDistance,
                   interactableMask))
            {
                if(!hit.collider.TryGetComponent(out ITargetable  target))
                    return;

                if(!target.IsAvailable())
                    return;

                TargetSelector.Instance.Select(target);
            }
        }

        public void EnableInput(bool value)
        {
            _inputEnabled = value;
        }
    }
}