using DOTE.Gameplay.Domain.Character;
using DOTE.Gameplay.Domain.Field;
using DOTE.Gameplay.Domain.Player;
using DOTE.Gameplay.Domain.SupportCard;
using DOTE.SharedKernel.Domain;
using System;

namespace DOTE.Gameplay.Domain.GameParty
{
    public interface IGameParty
    {
        public string GameId { get; }
        public AGamePartyState CurrentState { get; }
        public void SetStartGameState();
        public void SetEndGameState(string loserID);
        public void SetFirstPlayerTurnState();
        public void SetSecondPlayerTurnState();

        public void ChangePlayerTurnState();

        public void SetPlayerTurnStateState(Type type);
        public void SetPlayerTurnStateDefaultState();
        public void DecreasePlayerTurnPOI(int value);


        public void SelectCharacter(GamePartyPlayer player, PlayableCharacter character);
        public void DeselectCharacter(GamePartyPlayer player, PlayableCharacter character);
        public void MoveCharacter(GamePartyPlayer player, PlayableCharacter character, Hex targetCell, int moveCost);
        public void AttackCharacter(GamePartyPlayer player, PlayableCharacter attacker, PlayableCharacter target);
        public void UseCharacterAbility(GamePartyPlayer player, PlayableCharacter character, ActiveAbilityType abilityType);
        public void UseSupportCard(GamePartyPlayer player, ASupportCard supportCard);
    }
}