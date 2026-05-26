using DOTE.Gameplay.Domain.Character;
using DOTE.Gameplay.Domain.Field;
using DOTE.Gameplay.Domain.GameParty;
using DOTE.Gameplay.Domain.Player;
using DOTE.SharedKernel.Domain;
using System.Linq;

namespace DOTE.Gameplay.Application
{
    public class MoveCharacterCommandHandler : ACommandHandler<MoveCharacterCommand>
    {
        private IPlayerRepository playerRepository;
        private ICharacterRepository characterRepository;
        private IGamePartyRepository gamePartyRepository;
        private IFieldRepository fieldRepository;

        public MoveCharacterCommandHandler(IPlayerRepository playerRepository, ICharacterRepository characterRepository, IGamePartyRepository gamePartyRepository, IFieldRepository fieldRepository)
        {
            this.playerRepository = playerRepository;
            this.characterRepository = characterRepository;
            this.gamePartyRepository = gamePartyRepository;
            this.fieldRepository = fieldRepository;
        }

        public override void HandleHook(MoveCharacterCommand command)
        {
            GamePartyPlayer player = playerRepository.GetPlayerById(command.PlayerId);
            PlayableCharacter character = characterRepository.GetCharacterById(command.CharacterId);
            IGameParty gameParty = gamePartyRepository.GetCurrentIGameParty();
            Field field = fieldRepository.GetCurrentField();

            if (player != null && character != null && gameParty != null && field != null)
            {
                int moveCost = field.CalculatePathMoveCostForCharacter(command.MovePath, character);
                gameParty.CurrentState.MoveCharacter(player, character, command.MovePath.Last(), moveCost);
            }
        }
    }
}
