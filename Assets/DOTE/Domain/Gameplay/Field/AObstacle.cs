namespace DOTE.Gameplay.Domain.Field
{
    public abstract class AObstacle
    {
        public Hex Hex { get; private set; }

        public AObstacle(Hex hex)
        {
            Hex = hex;
        }
    }
}