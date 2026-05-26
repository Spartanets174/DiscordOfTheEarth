using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Application
{
    public class AttackCharacterCommand : ICommand
    {
        public string PlayerId { get; private set; }
        public string AttackerCharacterId { get; private set; }
        public string TargetCharacterId { get; private set; }

        public AttackCharacterCommand(string playerId, string attackerCharacterId, string targetCharacterId)
        {
            PlayerId = playerId;
            AttackerCharacterId = attackerCharacterId;
            TargetCharacterId = targetCharacterId;
        }
    }
}
