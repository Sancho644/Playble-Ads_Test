namespace Gameplay
{
    public sealed class TargetCleanupSystem
    {
        private readonly TargetComponent _playerTarget;

        public TargetCleanupSystem(TargetComponent playerTarget)
        {
            _playerTarget = playerTarget;
        }

        public void Tick(int frameCount)
        {
            if (_playerTarget == null || !_playerTarget.ShouldClear(frameCount))
            {
                return;
            }

            _playerTarget.Clear();
        }
    }
}
