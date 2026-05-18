namespace Gameplay
{
    public sealed class TapMoveCommandService
    {
        private readonly MoveTargetComponent _moveTarget;
        private readonly TargetComponent _targetComponent;

        public TapMoveCommandService(MoveTargetComponent moveTarget, TargetComponent targetComponent)
        {
            _moveTarget = moveTarget;
            _targetComponent = targetComponent;
        }

        public void SetDestination(UnityEngine.Vector3 destination)
        {
            if (_moveTarget == null)
            {
                return;
            }

            _targetComponent?.Clear();
            _moveTarget.SetDestination(destination);
        }
    }
}
