using DOTE.Gameplay.Domain.Character;
using DOTE.Gameplay.Domain.Field;
using DOTE.Gameplay.Domain.GameParty;
using DOTE.Gameplay.Domain.Item;
using DOTE.Gameplay.Domain.Player;
using DOTE.Gameplay.Domain.SupportCard;
using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Application
{
    public class GamePartyService
    {
        private IGamePartyRepository gamePartyRepository;
        private IPlayerRepository playerRepository;
        private ICharacterRepository characterRepository;
        private ISupportCardRepository supportCardRepository;
        private IFieldRepository fieldRepository;
        private IItemRepository itemRepository;

        private IDomainEventBus eventBus;

        public GamePartyService(IGamePartyRepository gamePartyRepository, IPlayerRepository playerRepository, ICharacterRepository characterRepository, ISupportCardRepository supportCardRepository, IFieldRepository fieldRepository, IItemRepository itemRepository, IDomainEventBus eventBus)
        {
            this.gamePartyRepository = gamePartyRepository;
            this.playerRepository = playerRepository;
            this.characterRepository = characterRepository;
            this.supportCardRepository = supportCardRepository;
            this.fieldRepository = fieldRepository;
            this.itemRepository = itemRepository;
            this.eventBus = eventBus;

            SubscribeToEvents();
        }

        ~GamePartyService()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            eventBus.Subscribe<CharacterMoved>(OnCharacterMoved);
            eventBus.Subscribe<CharacterDied>(OnCharacterDied);
        }

        private void UnsubscribeFromEvents()
        {
            eventBus.Unsubscribe<CharacterMoved>(OnCharacterMoved);
            eventBus.Unsubscribe<CharacterDied>(OnCharacterDied);
        }


        private void OnCharacterMoved(CharacterMoved moved)
        {
            Field field = fieldRepository.GetCurrentField();

            if (field != null)
            {
                field.FreeCell(moved.GetFromCellId());
                field.PlaceOnCell(moved.GetToCellId(), typeof(PlayableCharacter), moved.GetMovedCharacterId());
            }
        }

        private void OnCharacterDied(CharacterDied died)
        {
            PlayableCharacter character = characterRepository.GetCharacterById(died.GetDiedCharacterId());

            if (character == null)
            {
                return;
            }

            GamePartyPlayer player = playerRepository.GetPlayerById(character.OwnerId);

            if (player != null)
            {
                player.SetCharacterAliveStatus(character.CharacterId, false);
            }
        }

    }
}
