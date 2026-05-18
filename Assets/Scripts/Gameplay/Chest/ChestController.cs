using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Chest
{
    public class ChestController : MonoBehaviour, ITargetable
    {
        [SerializeField] private int bonusPower;
        
        public Transform GetTransform()
        {
            throw new System.NotImplementedException();
        }

        public int GetPower()
        {
            throw new System.NotImplementedException();
        }

        public void Interact(PlayerController player)
        {
            player.AddPower(bonusPower);

            Open();
        }

        public bool IsAvailable()
        {
            throw new System.NotImplementedException();
        }

        public float GetInteractionDistance()
        {
            throw new System.NotImplementedException();
        }

        public void Open()
        {
            
        }
    }
}