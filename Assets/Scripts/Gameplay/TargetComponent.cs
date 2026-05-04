using UnityEngine;

namespace Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntityIdentity))]
    public sealed class TargetComponent : MonoBehaviour
    {
        private EntityIdentity currentTarget;
        private int selectionVersion;
        private bool clearRequested;
        private int clearRequestedFrame = -1;

        public EntityIdentity CurrentTarget => currentTarget;
        public bool HasTarget => currentTarget != null;
        public int SelectionVersion => selectionVersion;
        public bool ClearRequested => clearRequested;

        private void Awake()
        {
            ResetRuntimeState();
        }

        public void Set(EntityIdentity target)
        {
            currentTarget = target;
            clearRequested = false;
            clearRequestedFrame = -1;
            selectionVersion++;
        }

        public void Clear()
        {
            currentTarget = null;
            clearRequested = false;
            clearRequestedFrame = -1;
            selectionVersion++;
        }

        public void RequestClear(int frameCount)
        {
            clearRequested = true;
            clearRequestedFrame = frameCount;
        }

        public bool ShouldClear(int frameCount)
        {
            return clearRequested && clearRequestedFrame >= 0 && clearRequestedFrame < frameCount;
        }

        public bool IsSelected(EntityIdentity target)
        {
            return currentTarget == target;
        }

        public void ResetRuntimeState()
        {
            currentTarget = null;
            selectionVersion = 0;
            clearRequested = false;
            clearRequestedFrame = -1;
        }
    }
}
