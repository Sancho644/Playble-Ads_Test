using Gameplay;

namespace Core.States
{
    public class MovingToTargetState : AbstractGameState
    {
        public override void EnterState()
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }
        
        public void MoveToTarget(ITargetable target)
        {
            //movement.MoveTo(target.GetTransform().position);
        }
    }
}