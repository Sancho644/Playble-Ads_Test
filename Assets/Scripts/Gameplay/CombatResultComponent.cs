using UnityEngine;

namespace Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntityIdentity))]
    public sealed class CombatResultComponent : MonoBehaviour
    {
        private CombatResultState result;
        private EntityIdentity resolvedTarget;
        private int powerDelta;
        private int processedSelectionVersion = -1;
        private int resultVersion;
        private int appliedResultVersion = -1;

        public CombatResultState Result => result;
        public EntityIdentity ResolvedTarget => resolvedTarget;
        public int PowerDelta => powerDelta;
        public int ProcessedSelectionVersion => processedSelectionVersion;
        public int ResultVersion => resultVersion;
        public int AppliedResultVersion => appliedResultVersion;
        public bool HasPendingApplication => resultVersion != appliedResultVersion;

        private void Awake()
        {
            ResetRuntimeState();
        }

        public void SetResult(
            CombatResultState result,
            EntityIdentity resolvedTarget,
            int powerDelta,
            int processedSelectionVersion)
        {
            this.result = result;
            this.resolvedTarget = resolvedTarget;
            this.powerDelta = powerDelta;
            this.processedSelectionVersion = processedSelectionVersion;
            resultVersion++;
        }

        public void MarkApplied()
        {
            appliedResultVersion = resultVersion;
        }

        public void ResetRuntimeState()
        {
            result = CombatResultState.None;
            resolvedTarget = null;
            powerDelta = 0;
            processedSelectionVersion = -1;
            resultVersion = 0;
            appliedResultVersion = 0;
        }
    }
}
