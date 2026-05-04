using UnityEngine;

namespace Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntityIdentity))]
    public sealed class MoveTargetComponent : MonoBehaviour
    {
        private Vector3 destination;
        private bool hasDestination;
        private int commandVersion;

        public Vector3 Destination => destination;
        public bool HasDestination => hasDestination;
        public int CommandVersion => commandVersion;

        private void Awake()
        {
            ResetRuntimeState();
        }

        public void SetDestination(Vector3 destination)
        {
            this.destination = destination;
            hasDestination = true;
            commandVersion++;
        }

        public void Clear()
        {
            hasDestination = false;
            commandVersion++;
        }

        public void ResetRuntimeState()
        {
            destination = default;
            hasDestination = false;
            commandVersion = 0;
        }
    }
}
