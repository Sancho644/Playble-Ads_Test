using Gameplay.Player;
using Systems;
using UnityEngine;

namespace Gameplay.Enemy
{
    public class EnemyController : MonoBehaviour, ITargetable
    {
        [SerializeField] private int power;
        [SerializeField] private Animator animator;

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
            if(player.Power >= power)
            {
                player.AddPower(power);

                Die();

                LevelSystem.Instance.NotifyEnemyKilled(this);
            }
            else
            {
                player.PlayFailFeedback();
            }
        }

        public bool IsAvailable()
        {
            throw new System.NotImplementedException();
        }

        public float GetInteractionDistance()
        {
            throw new System.NotImplementedException();
        }

        public void Die()
        {
            
        }
    }
}