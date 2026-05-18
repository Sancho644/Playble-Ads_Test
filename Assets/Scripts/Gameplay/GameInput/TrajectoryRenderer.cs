using UnityEngine;

namespace Gameplay.GameInput
{
    public class TrajectoryRenderer
    {
        [SerializeField] private Transform player;
        [SerializeField] private LineRenderer line;
        
        private ITargetable _currentTarget;

        public void Show(Transform from, Transform to)
        {
            line.enabled = true;

            line.SetPosition(0, from.position);
            line.SetPosition(1, to.position);
        }
        
        private void Update()
        {
            if(!line.enabled)
                return;

            if(_currentTarget == null)
                return;

            var targetPosition = _currentTarget.GetTransform().position;
            
            line.SetPosition(0, player.position);
            line.SetPosition(1, targetPosition);
        }
        
        public void Hide()
        {
            line.enabled = false;
        }
    }
}