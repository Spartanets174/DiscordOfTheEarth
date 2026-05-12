using DOTE.Gameplay.Domain.Character;
using DOTE.Gameplay.Domain.Player;
using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.GameParty
{
    public class GamePartyDomainService
    {
        private ICharacterRepository characterRepository;
        private IPlayerRepository playerRepository;
        private IGameParty currentGameParty;
        private IDomainEventBus eventBus;

        public GamePartyDomainService(IGameParty currentGameParty, IDomainEventBus eventBus, IPlayerRepository playerRepository, ICharacterRepository characterRepository)
        {
            this.currentGameParty = currentGameParty;
            this.eventBus = eventBus;
            this.playerRepository = playerRepository;
            this.characterRepository = characterRepository;

            this.eventBus.Subscribe<PointsOfActionValueChanged>(OnPlayerTurnPOAChanged);
            this.eventBus.Subscribe<PlayerAliveCharactersCountChanged>(OnPlayerAliveCharactersCountChanged);

            this.eventBus.Subscribe<CharacterMoved>(OnCharacterMoved);
            this.eventBus.Subscribe<CharacterAttacked>(OnCharacterAttacked);
            this.eventBus.Subscribe<CharacterActiveAbilityUsed>(OnActiveAbilityUsed);
        }



        ~GamePartyDomainService()
        {
            eventBus.Unsubscribe<PointsOfActionValueChanged>(OnPlayerTurnPOAChanged);
            eventBus.Unsubscribe<PlayerAliveCharactersCountChanged>(OnPlayerAliveCharactersCountChanged);

            eventBus.Unsubscribe<CharacterMoved>(OnCharacterMoved);
            eventBus.Unsubscribe<CharacterAttacked>(OnCharacterAttacked);
            eventBus.Unsubscribe<CharacterActiveAbilityUsed>(OnActiveAbilityUsed);
        }

        private void OnPlayerTurnPOAChanged(PointsOfActionValueChanged domainEvent)
        {
            if (domainEvent.GetPointsOfAction() == 0)
            {
                currentGameParty.ChangePlayerTurnState();
            }
        }

        private void OnPlayerAliveCharactersCountChanged(PlayerAliveCharactersCountChanged domainEvent)
        {
            if (domainEvent.GetCount() == 0)
            {
                currentGameParty.SetEndGameState(domainEvent.GetPlayerId());
            }
        }

        private void OnCharacterMoved(CharacterMoved moved)
        {
            currentGameParty.DecreasePlayerTurnPOI(moved.GetMoveCost());
        }

        private void OnCharacterAttacked(CharacterAttacked attacked)
        {
            if (currentGameParty.CurrentState is PlayerTurnState playerTurnState)
            {
                GamePartyPlayer player = playerRepository.GetPlayerById(playerTurnState.PlayerId);
                if (player == null)
                {
                    return;
                }

                if (!player.HasCharacter(attacked.GetAttackerId()))
                {
                    return;
                }

                currentGameParty.DecreasePlayerTurnPOI(attacked.GetAttackCost());
            }
        }

        private void OnActiveAbilityUsed(CharacterActiveAbilityUsed used)
        {
            if (currentGameParty.CurrentState is PlayerTurnState playerTurnState)
            {
                GamePartyPlayer player = playerRepository.GetPlayerById(playerTurnState.PlayerId);
                PlayableCharacter character = characterRepository.GetCharacterById(used.GetCharacterId());

                if (player == null || character == null)
                {
                    return;
                }

                if (!player.HasCharacter(character.CharacterId))
                {
                    return;
                }

                currentGameParty.DecreasePlayerTurnPOI(character.UseAbilityCost.CurrentValue);
            }
        }
    }
}