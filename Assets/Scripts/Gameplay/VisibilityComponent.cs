using UnityEngine;

namespace Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntityIdentity))]
    public sealed class VisibilityComponent : MonoBehaviour
    {
        private bool isHidden;

        public bool IsHidden => isHidden;

        private void Awake()
        {
            isHidden = false;
        }

        public void Hide()
        {
            isHidden = true;
        }

        public void Show()
        {
            isHidden = false;
        }
    }
}
