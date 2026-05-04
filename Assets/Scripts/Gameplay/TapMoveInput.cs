using View;
using UnityEngine;

namespace Gameplay
{
    public sealed class TapMoveInput : MonoBehaviour
    {
        [SerializeField] private Camera worldCamera;
        [SerializeField] private LayerMask blockedLayerMask;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private MoveTargetComponent moveTarget;
        [SerializeField] private TargetComponent playerTarget;

        private TapMoveCommandService _commandService;
        private TargetSelectionRaycaster _targetRaycaster;
        private readonly TargetSelectionView[] _blockedViews = new TargetSelectionView[8];

        private void Awake()
        {
            if (worldCamera == null)
            {
                worldCamera = Camera.main;
            }

            if (playerTransform == null && moveTarget != null)
            {
                playerTransform = moveTarget.transform;
            }

            _commandService = new TapMoveCommandService(moveTarget, playerTarget);
            _targetRaycaster = new TargetSelectionRaycaster(worldCamera, blockedLayerMask);
        }

        private void Update()
        {
            if (worldCamera == null || _commandService == null)
            {
                return;
            }

            if (!PointerInputUtility.TryGetPointerDownPosition(out var screenPosition))
            {
                return;
            }

            if (HasBlockedTarget(screenPosition))
            {
                return;
            }

            if (!TryGetWorldPosition(screenPosition, out var worldPosition))
            {
                return;
            }

            _commandService.SetDestination(worldPosition);
        }

        private bool HasBlockedTarget(Vector2 screenPosition)
        {
            if (blockedLayerMask.value == 0)
            {
                return false;
            }

            var hitCount = _targetRaycaster.Raycast(screenPosition, _blockedViews);
            for (var i = 0; i < hitCount; i++)
            {
                if (_blockedViews[i] != null)
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryGetWorldPosition(Vector2 screenPosition, out Vector3 worldPosition)
        {
            if (worldCamera == null || playerTransform == null)
            {
                worldPosition = default;
                return false;
            }

            var ray = worldCamera.ScreenPointToRay(screenPosition);
            var movePlane = new Plane(Vector3.up, new Vector3(0f, playerTransform.position.y, 0f));
            if (!movePlane.Raycast(ray, out var distance))
            {
                worldPosition = default;
                return false;
            }

            worldPosition = ray.GetPoint(distance);
            worldPosition.y = playerTransform.position.y;
            return true;
        }
    }
}
