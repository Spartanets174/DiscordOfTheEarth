using DOTE.Gameplay.Domain.GameParty;

namespace DOTE.Gameplay.Infrastructure
{
    public class GamePartyRepository : IGamePartyRepository
    {
        private IGameParty currentGameParty;

        public GamePartyRepository(IGameParty currentGameParty)
        {
            this.currentGameParty = currentGameParty;
        }

        public IGameParty GetCurrentIGameParty()
        {
            return currentGameParty;
        }
    }
}
