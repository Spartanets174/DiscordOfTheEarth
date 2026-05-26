using DOTE.Gameplay.Domain.Character;
using DOTE.Gameplay.Domain.GameParty;
using DOTE.Gameplay.Domain.Player;
using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Application
{
    public class AttackCharacterCommandHandler : ACommandHandler<AttackCharacterCommand>
    {
        private IPlayerRepository playerRepository;
        private ICharacterRepository characterRepository;
        private IGamePartyRepository gamePartyRepository;

        public AttackCharacterCommandHandler(IPlayerRepository playerRepository, ICharacterRepository characterRepository, IGamePartyRepository gamePartyRepository)
        {
            this.playerRepository = playerRepository;
            this.characterRepository = characterRepository;
            this.gamePartyRepository = gamePartyRepository;
        }

        public override void HandleHook(AttackCharacterCommand command)
        {
            GamePartyPlayer player = playerRepository.GetPlayerById(command.PlayerId);
            PlayableCharacter attacker = characterRepository.GetCharacterById(command.AttackerCharacterId);
            PlayableCharacter target = characterRepository.GetCharacterById(command.TargetCharacterId);
            IGameParty gameParty = gamePartyRepository.GetCurrentIGameParty();

            if (player != null && attacker != null && target != null && gameParty != null)
            {
                gameParty.CurrentState.AttackCharacter(player, attacker, target);
            }
        }
    }
}
