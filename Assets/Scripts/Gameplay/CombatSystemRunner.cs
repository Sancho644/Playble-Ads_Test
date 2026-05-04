using UnityEngine;

namespace Gameplay
{
    public sealed class CombatSystemRunner : MonoBehaviour
    {
        [SerializeField] private EntityIdentity playerIdentity;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Collider playerCollider;
        [SerializeField] private ActorPower playerPower;
        [SerializeField] private TargetComponent playerTarget;
        [SerializeField] private CombatResultComponent combatResult;
        [SerializeField] private VisualFeedbackEventComponent visualFeedback;

        private CombatResolutionSystem _combatResolutionSystem;
        private CombatResultApplySystem _combatResultApplySystem;
        private TargetCleanupSystem _targetCleanupSystem;

        private void Awake()
        {
            if (playerTransform == null && playerIdentity != null)
            {
                playerTransform = playerIdentity.transform;
            }

            if (playerCollider == null && playerIdentity != null)
            {
                playerCollider = playerIdentity.GetComponent<Collider>();
            }

            var combatProximityService = new CombatProximityService(playerTransform, playerCollider);

            _combatResolutionSystem = new CombatResolutionSystem(
                playerIdentity,
                playerPower,
                playerTarget,
                combatResult,
                combatProximityService);

            _combatResultApplySystem = new CombatResultApplySystem(
                playerPower,
                playerTarget,
                combatResult,
                visualFeedback);

            _targetCleanupSystem = new TargetCleanupSystem(playerTarget);
        }

        private void Update()
        {
            _combatResolutionSystem?.Tick();
            _combatResultApplySystem?.Tick();
            _targetCleanupSystem?.Tick(Time.frameCount);
        }
    }
}
