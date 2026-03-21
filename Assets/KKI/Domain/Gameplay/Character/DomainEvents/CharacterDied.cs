namespace DOTE.Domain.Gameplay.Character
{
    public class CharacterDied 
    {
        private string diedCharacterId;

        public CharacterDied(string diedCharacterId)
        {
            this.diedCharacterId = diedCharacterId;
        }

        public string GetDiedCharacterId() => diedCharacterId;
    }
}