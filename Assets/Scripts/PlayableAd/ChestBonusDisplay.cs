using Gameplay;
using UnityEngine;

namespace PlayableAd
{
    /// <summary>
    /// Хранит бонусное значение сундука для отображения в UI.
    /// ActorPower для InteractiveObject всегда 0 (OnValidate), поэтому
    /// бонус хранится здесь и читается WorldSpacePowerLabel.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ChestBonusDisplay : MonoBehaviour
    {
        [SerializeField] public int bonusValue = 5;

        public int BonusValue => bonusValue;
    }
}
