namespace Contests.Models.Factories
{
    using System;
    using Common;
    using Enums;
    using Interfaces;
    using Strategies;
    using Strategies.RewardStrategy;
   
    public class RewardFactory
    {
        public static RewardStrategy CreateStrategy(RewardType strategyType, RewardModel model)
        {
            switch (strategyType)
            {
                case RewardType.SingleWinner:
                    return new SingleWinner();
                case RewardType.TopNWinners:
                    // ReSharper disable once PossibleInvalidOperationException
                    byte winnersCount = (byte) model.WinnersCount;
                    return new TopNWinners(winnersCount);
                default:
                    throw new ArgumentException();
            }
        }
    }
}
