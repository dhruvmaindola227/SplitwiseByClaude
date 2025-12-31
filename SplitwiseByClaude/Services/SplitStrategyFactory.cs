
using static SplitwiseByClaude.Entities.Enum;

namespace SplitwiseByClaude.Services
{
    public static class SplitStrategyFactory
    {
        public static ISplitStrategy GetStrategy(SplitType splitType)
        {
           switch (splitType)
           {
                case SplitType.EqualSplit:
                    return new EqualSplitStrategy();
                case SplitType.ExactSplit:
                    return new ExactSplitStrategy();
                case SplitType.PercentageSplit:
                    return new PercentageSplitStrategy();
                default:
                    throw new ArgumentException("Invalid split type.");
            }
        }
    }
}
