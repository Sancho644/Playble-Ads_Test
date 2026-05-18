using UnityEngine;

namespace Gameplay
{
    [DisallowMultipleComponent]
    public sealed class EntityIdentity : MonoBehaviour
    {
        [SerializeField] private EntityKind kind;

        public EntityKind Kind => kind;
        public bool IsPlayer => kind == EntityKind.Player;
        public bool IsEnemy => kind == EntityKind.Enemy;
        public bool IsInteractiveObject => kind == EntityKind.InteractiveObject;
        public bool HasPower => kind != EntityKind.InteractiveObject;
    }
}
