using UnityEngine;

namespace Gameplay
{
    public sealed class PlayerMovementRunner : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private MoveTargetComponent moveTarget;
        [SerializeField] private MoveSpeedComponent moveSpeed;
        [SerializeField] private MovementVisualStateComponent movementVisualState;
        [SerializeField] private float stopDistance = 0.05f;

        private PlayerMovementSystem _movementSystem;

        private void Awake()
        {
            if (playerTransform == null)
            {
                playerTransform = transform;
            }

            if (moveTarget == null && playerTransform != null)
            {
                moveTarget = playerTransform.GetComponent<MoveTargetComponent>();
            }

            if (moveSpeed == null && playerTransform != null)
            {
                moveSpeed = playerTransform.GetComponent<MoveSpeedComponent>();
            }

            if (movementVisualState == null && playerTransform != null)
            {
                movementVisualState = playerTransform.GetComponent<MovementVisualStateComponent>();
            }

            _movementSystem = new PlayerMovementSystem(
                playerTransform,
                moveTarget,
                moveSpeed,
                movementVisualState,
                stopDistance);
        }

        private void Update()
        {
            _movementSystem?.Tick(Time.deltaTime);
        }
    }
}
