using UnityEngine;

namespace Gameplay
{
    public sealed class TargetSelectionInput : MonoBehaviour
    {
        [SerializeField] private Camera worldCamera;
        [SerializeField] private LayerMask targetLayerMask = ~0;
        [SerializeField] private EntityIdentity playerIdentity;
        [SerializeField] private TargetComponent playerTarget;
        [SerializeField] private MoveTargetComponent moveTarget;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Collider playerCollider;

        private TargetSelectionProcessor _processor;

        private void Awake()
        {
            if (worldCamera == null)
            {
                worldCamera = Camera.main;
            }

            if (playerTransform == null && playerIdentity != null)
            {
                playerTransform = playerIdentity.transform;
            }

            if (playerCollider == null && playerIdentity != null)
            {
                playerCollider = playerIdentity.GetComponent<Collider>();
            }

            var approachPositionService = new ApproachPositionService(playerTransform, playerCollider);
            var selectionService = new TargetSelectionService(playerIdentity, playerTarget, moveTarget, approachPositionService);
            _processor = new TargetSelectionProcessor(worldCamera, targetLayerMask, selectionService);
        }

        private void Update()
        {
            if (_processor == null)
            {
                return;
            }

            if (!PointerInputUtility.TryGetPointerDownPosition(out var screenPosition))
            {
                return;
            }

            _processor.ProcessPointerDown(screenPosition);
        }
    }
}
