using Gameplay.Enemy;
using UnityEngine;

namespace Systems
{
    public class LevelSystem : MonoBehaviour
    {
        public static LevelSystem Instance;
        
        private void Awake()
        {
            Instance = this;
        }
        
        public void InitializeLevel()
        {
            
        }

        public void CheckVictory()
        {
            /*if(aliveEnemies <= 0)
            {
                GameManager.Instance.ChangeState(GameState.Victory);
            }
            else
            {
                GameManager.Instance.ChangeState(GameState.WaitingInput);
            }*/
        }

        public void NotifyEnemyKilled(EnemyController enemyController)
        {
            throw new System.NotImplementedException();
        }
    }
}