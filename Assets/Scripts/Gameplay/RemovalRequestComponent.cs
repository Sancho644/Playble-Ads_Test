using UnityEngine;

namespace Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntityIdentity))]
    public sealed class RemovalRequestComponent : MonoBehaviour
    {
        private bool isRequested;

        public bool IsRequested => isRequested;

        private void Awake()
        {
            isRequested = false;
        }

        public void Request()
        {
            isRequested = true;
        }

        public void Clear()
        {
            isRequested = false;
        }
    }
}
