using Gameplay.Player;
using UnityEngine;

namespace Gameplay.GameInput
{
    public class TargetSelector
    {
        [SerializeField] private TrajectoryRenderer trajectory;
        [SerializeField] private TargetIndicator indicator;
        
        public static TargetSelector Instance;

        private ITargetable currentTarget;
        
        private void Awake()
        {
            Instance = this;
        }
        
        public void Select(ITargetable target)
        {
            if(currentTarget == target)
                return;

            DeselectCurrent();

            currentTarget = target;

            indicator.Show(target.GetTransform());

            trajectory.Show(PlayerController.Instance.transform, target.GetTransform());

            PlayerController.Instance.SetTarget(target);
        }
        
        public void DeselectCurrent()
        {
            indicator.Hide();

            trajectory.Hide();

            currentTarget = null;
        }
    }
}