using DOTE.Gameplay.Domain.SupportCard;
using DOTE.SharedKernel.Infrastructure;

namespace DOTE.Gameplay.Infrastructure
{
    public interface ISupportCardBuilder
    {
        public abstract ASupportCard Build(string id, ASupportCardConfig supportCardConfig);
    }

    public abstract class ASupportCardBuilder<TSupportCard, TConfig> : ISupportCardBuilder where TSupportCard : ASupportCard where TConfig : ASupportCardConfig
    {
        public abstract TSupportCard Build(string id, TConfig config);
        ASupportCard ISupportCardBuilder.Build(string id, ASupportCardConfig supportCardConfig)
        {
            return Build(id, supportCardConfig as TConfig);
        }
    }
}