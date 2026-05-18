using Gameplay.Player;
using UnityEngine;

namespace Gameplay
{
    public interface ITargetable
    {
        public Transform GetTransform();
        public int GetPower();
        public void Interact(PlayerController player);
        public bool IsAvailable();
        public float GetInteractionDistance();
    }
}