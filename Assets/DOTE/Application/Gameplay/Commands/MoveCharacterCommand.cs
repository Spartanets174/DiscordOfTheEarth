using DOTE.Gameplay.Domain.Field;
using DOTE.SharedKernel.Domain;
using System.Collections.Generic;

namespace DOTE.Gameplay.Application
{
    public class MoveCharacterCommand : ICommand
    {
        public string PlayerId { get; private set; }
        public string CharacterId { get; private set; }
        public List<Hex> MovePath { get; private set; }

        public MoveCharacterCommand(string playerId, string characterId, List<Hex> movePath)
        {
            PlayerId = playerId;
            CharacterId = characterId;
            MovePath = movePath;
        }
    }
}
