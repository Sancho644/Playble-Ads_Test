using UnityEngine;

namespace Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntityIdentity))]
    public sealed class MovementVisualStateComponent : MonoBehaviour
    {
        private bool isMoving;
        private Vector3 facingDirection = Vector3.forward;

        public bool IsMoving => isMoving;
        public Vector3 FacingDirection => facingDirection;

        public void SetMoving(Vector3 direction)
        {
            var planarDirection = new Vector3(direction.x, 0f, direction.z);
            if (planarDirection.sqrMagnitude > 0.0001f)
            {
                facingDirection = planarDirection.normalized;
            }

            isMoving = true;
        }

        public void Stop()
        {
            isMoving = false;
        }
    }
}
