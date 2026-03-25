using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.Character
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