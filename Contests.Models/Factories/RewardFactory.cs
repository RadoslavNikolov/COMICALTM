namespace Contests.Models.Factories
{
    using System;
    using Enums;
    using Interfaces;
    using Strategies;
    using Strategies.RewardStrategy;
   
    public class RewardFactory
    {
        public static RewardStrategy CreateStrategy(RewardType strategyType)
        {
            switch (strategyType)
            {
                case RewardType.SingleWinner:
                    return new SingleWinner();
                case RewardType.TopNWinners:
                    return new TopNWinners(1);
                default:
                    throw new ArgumentException();
            }
        }
    }
}
