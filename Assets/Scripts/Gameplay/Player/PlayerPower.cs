using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerPower : MonoBehaviour
    {
        [SerializeField] private int power;

        public int Power => power;

        public void AddPower(int value)
        {
        }

        public void CanDefeat(int targetPower)
        {
        }
    }
}