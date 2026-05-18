using Gameplay;
using Gameplay.Enemy;
using Gameplay.Player;
using UnityEngine;

namespace Systems
{
    public class CombatSystem : MonoBehaviour
    {
        public void TryAttack(PlayerController player, EnemyController enemy)
        {
            
        }
        
        public void ResolveInteraction(PlayerController player, ITargetable target)
        {
            target.Interact(player);
        }
    }
}