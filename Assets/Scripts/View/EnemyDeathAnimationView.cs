using Gameplay;
using UnityEngine;

namespace View
{
    public sealed class EnemyDeathAnimationView : MonoBehaviour
    {
        [SerializeField] private RemovalRequestComponent removalRequest;
        [SerializeField] private VisibilityComponent visibility;
        [SerializeField] private Animator animator;
        [SerializeField] private string idleStateName = "Idle";
        [SerializeField] private string deathStateName = "Death";
        [SerializeField] private string deathTriggerParameter = "Die";
        [SerializeField] private string deathBoolParameter;
        [SerializeField] private float fallbackDuration = 1.2f;

        private EnemyDeathAnimationPresenter _presenter;

        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>(true);
            }

            _presenter = new EnemyDeathAnimationPresenter(
                removalRequest,
                visibility,
                animator,
                idleStateName,
                deathStateName,
                deathTriggerParameter,
                deathBoolParameter,
                fallbackDuration);

            _presenter.PlayIdle();
        }

        private void LateUpdate()
        {
            _presenter?.Refresh(Time.deltaTime);
        }
    }
}
