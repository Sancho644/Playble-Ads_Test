using System;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float rotationSpeed = 10f;
        [Header("References")]
        [SerializeField] private Transform visualRoot;
        [SerializeField] private Animator animator;

        private Vector3 targetPosition;
        private bool isMoving;
        private Action onReached;
        private float _stopDistance;

        private static readonly int Run = Animator.StringToHash("Run");
        
        private void Update()
        {
            if(!isMoving)
                return;

            Move();
            Rotate();
        }
        
        public void MoveTo(Vector3 destination, Action reachedCallback)
        {
            targetPosition = destination;
            onReached = reachedCallback;
            isMoving = true;

            animator.SetBool(Run, true);
        }

        public void Stop()
        {
            isMoving = false;

            animator.SetBool(Run, false);
        }

        public void SetStopDistance(float stopDistance)
        {
            _stopDistance = stopDistance;
        }

        private void Move()
        {
            var direction = targetPosition - transform.position;

            direction.y = 0f;

            var distance = direction.magnitude;

            if(distance <= _stopDistance)
            {
                ReachDestination();

                return;
            }

            direction.Normalize();

            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        private void Rotate()
        {
            var direction = targetPosition - transform.position;

            direction.y = 0f;

            if(direction.sqrMagnitude <= 0.001f)
                return;

            var targetRotation = Quaternion.LookRotation(direction);

            visualRoot.rotation = Quaternion.Slerp(
                    visualRoot.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime);
        }

        private void ReachDestination()
        {
            isMoving = false;

            animator.SetBool(Run, false);

            onReached?.Invoke();
        }
    }
}