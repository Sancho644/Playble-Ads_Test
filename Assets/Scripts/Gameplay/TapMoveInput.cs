using View;
using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// Обрабатывает тап по земле — двигает игрока к точке клика.
    /// Тапы по врагам и интерактивным объектам передаются TargetSelectionInput.
    /// </summary>
    public sealed class TapMoveInput : MonoBehaviour
    {
        [SerializeField] private Camera worldCamera;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private MoveTargetComponent moveTarget;
        [SerializeField] private TargetComponent playerTarget;

        private TapMoveCommandService _commandService;

        private void Awake()
        {
            if (worldCamera == null)
                worldCamera = Camera.main;

            if (playerTransform == null && moveTarget != null)
                playerTransform = moveTarget.transform;

            _commandService = new TapMoveCommandService(moveTarget, playerTarget);
        }

        private void Update()
        {
            if (worldCamera == null || _commandService == null) return;
            if (!PointerInputUtility.TryGetPointerDownPosition(out var screenPosition)) return;

            // Если тап по врагу или интерактивному объекту — не обрабатываем
            if (HitInteractiveTarget(screenPosition)) return;

            // Вычислить точку на земле
            if (!TryGetGroundPosition(screenPosition, out var worldPosition)) return;

            _commandService.SetDestination(worldPosition);
        }

        private bool HitInteractiveTarget(Vector2 screenPosition)
        {
            var ray = worldCamera.ScreenPointToRay(screenPosition);
            var hits = Physics.RaycastAll(ray, float.MaxValue);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (var hit in hits)
            {
                if (hit.collider == null) continue;

                var tsv = hit.collider.GetComponent<TargetSelectionView>();
                if (tsv == null)
                    tsv = hit.collider.GetComponentInParent<TargetSelectionView>();

                if (tsv == null) continue;

                var identity = tsv.EntityIdentity;
                if (identity == null) continue;

                if (identity.IsEnemy || identity.IsInteractiveObject)
                    return true;

                if (identity.IsPlayer)
                    continue;

                if (!tsv.AllowClickThrough)
                    return true;
            }

            return false;
        }

        private bool TryGetGroundPosition(Vector2 screenPosition, out Vector3 worldPosition)
        {
            if (worldCamera == null)
            {
                worldPosition = default;
                return false;
            }

            var ray = worldCamera.ScreenPointToRay(screenPosition);
            float groundY = playerTransform != null ? playerTransform.position.y : 0f;

            // Математическая горизонтальная плоскость на высоте игрока
            var movePlane = new Plane(Vector3.up, new Vector3(0f, groundY, 0f));

            if (movePlane.Raycast(ray, out var distance) && distance > 0f)
            {
                worldPosition = ray.GetPoint(distance);
                worldPosition.y = groundY;
                return true;
            }

            worldPosition = default;
            return false;
        }
    }
}
