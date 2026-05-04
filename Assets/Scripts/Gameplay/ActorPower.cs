using UnityEngine;

namespace Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntityIdentity))]
    public sealed class ActorPower : MonoBehaviour
    {
        [SerializeField] private int value = 1;

        public int Value => value;

        public void SetValue(int value)
        {
            this.value = value;
        }

        public void Add(int amount)
        {
            value += amount;
        }

        private void OnValidate()
        {
            var identity = GetComponent<EntityIdentity>();
            if (identity != null && identity.IsInteractiveObject)
            {
                value = 0;
            }
        }
    }
}
