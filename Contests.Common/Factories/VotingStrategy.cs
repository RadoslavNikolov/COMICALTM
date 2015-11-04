namespace Contests.Common.Factories
{
    using System;
    using Common;
    using Models.Enums;
    using Models.Strategies.VotingStrategy;

    public class VotingFactory
    {
        public static VotingStrategy CreateStrategy(VotingType votingType, VotingModel model)
        {
            switch (votingType)
            {
                case VotingType.Open:
                    return new OpenVotingStrategy(model.Contest, model.Photo, model.UserId);
                case VotingType.Closed:
                    return new ClosedVotingStrategy(model.Contest, model.Photo, model.UserId);
                default:
                    throw new ArgumentException();
            }
        }
    }
}
