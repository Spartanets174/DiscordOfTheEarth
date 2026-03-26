namespace DOTE.Gameplay.Domain.Character
{
    public interface ICharacterRepository
    {
        public void AddCharacter(PlayableCharacter character);
        public void RemoveCharacter(string characterId);
        public PlayableCharacter GetCharacterById(string characterId);
    }
}