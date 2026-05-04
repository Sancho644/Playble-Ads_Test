using Gameplay;
using UnityEngine;

namespace View
{
    public sealed class PlayerMovementVisualView : MonoBehaviour
    {
        [SerializeField] private MovementVisualStateComponent movementVisualState;
        [SerializeField] private Transform visualRoot;
        [SerializeField] private Animator animator;
        [SerializeField] private string idleStateName = "Idle";
        [SerializeField] private string moveStateName = "Move";
        [SerializeField] private string movingBoolParameter = "IsMoving";
        [SerializeField] private AnimationClip idleClip;
        [SerializeField] private AnimationClip runClip;
        [SerializeField] private float rotationSpeed = 720f;
        [SerializeField] private float yawOffset;

        private ICharacterAnimationDriver _animationDriver;
        private PlayerMovementVisualPresenter _presenter;

        private void Awake()
        {
            if (visualRoot == null)
            {
                visualRoot = transform;
            }

            if (animator == null && visualRoot != null)
            {
                animator = visualRoot.GetComponentInChildren<Animator>(true);
            }

            _animationDriver = CreateAnimationDriver();
            _presenter = new PlayerMovementVisualPresenter(
                movementVisualState,
                visualRoot,
                _animationDriver,
                rotationSpeed,
                yawOffset);
        }

        private void LateUpdate()
        {
            _presenter?.Refresh(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _animationDriver?.Dispose();
        }

        private ICharacterAnimationDriver CreateAnimationDriver()
        {
            if (animator != null && animator.runtimeAnimatorController != null)
            {
                return new AnimatorStateDriver(animator, idleStateName, moveStateName, movingBoolParameter);
            }

            return new CharacterAnimationPlayable(animator, idleClip, runClip);
        }
    }
}
