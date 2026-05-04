using UnityEngine;

namespace Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntityIdentity))]
    public sealed class MoveSpeedComponent : MonoBehaviour
    {
        [SerializeField] private float unitsPerSecond = 3f;

        public float UnitsPerSecond => unitsPerSecond;
    }
}
