using DOTE.Gameplay.Domain.Character;
using DOTE.Gameplay.Domain.Field;
using DOTE.Gameplay.Domain.Player;
using DOTE.SharedKernel.Domain;
using System.Collections.Generic;

namespace DOTE.Gameplay.Domain.GameParty
{
    public class PlayerTurnDefaultState : APlayerTurnState
    {
        public override string Name => nameof(PlayerTurnDefaultState);

        public override void SelectCharacter(GamePartyPlayer player, PlayableCharacter character)
        {
            List<string> selectedCharacters = player.GetSelectedCharacterIds();

            //¬ этом стейте нельз€ выбрать больше 1 персонажа
            if (selectedCharacters.Count >= 1)
            {
                foreach (var selectedCharacter in selectedCharacters)
                {
                    player.DeselectCharacter(selectedCharacter);
                }
            }

            player.SelectCharacter(character.CharacterId);
        }

        public override void DeselectCharacter(GamePartyPlayer player, PlayableCharacter character)
        {
            player.DeselectCharacter(character.CharacterId);
        }

        public override void MoveCharacter(GamePartyPlayer player, PlayableCharacter character, Hex targetCell, int moveCost)
        {
            player.MoveCharacter(character, targetCell, moveCost);
        }

        public override void AttackCharacter(GamePartyPlayer player, PlayableCharacter attacker, PlayableCharacter target)
        {
            player.AttackCharacter(attacker, target);
        }

        public override void UseCharacterAbility(GamePartyPlayer player, PlayableCharacter character, ActiveAbilityType abilityType)
        {
            player.UseCharacterAbility(character, abilityType);
        }
    }
}