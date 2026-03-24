using DOTE.Domain.DomainModel;

namespace DOTE.Domain.Gameplay.Character
{
    public class CharacterDied : IDomainEvent
    {
        private string diedCharacterId;

        public CharacterDied(string diedCharacterId)
        {
            this.diedCharacterId = diedCharacterId;
        }

        public string GetDiedCharacterId() => diedCharacterId;
    }
}