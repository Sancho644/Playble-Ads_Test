namespace View
{
    public interface ICharacterAnimationDriver
    {
        void SetMoving(bool isMoving);
        void Update(float deltaTime);
        void Dispose();
    }
}
