namespace Contests.Models.Factories
{
    using System;
    using System.Collections.Generic;
    using Enums;
    using Interfaces;
    using Microsoft.AspNet.Identity;
    using Strategies.ParticipationStrategy;

    public class ParticipationFactory
    {
        public static IParticipationStrategy CreateStrategy(ParticipationType participationType, ICollection<IUser> participants)
        {
            switch (participationType)
            {
                case ParticipationType.Open:
                    return new OpenParticipationStrategy();
                case ParticipationType.Close:
                    return new ClosedParticipationStrategy(participants);
                default:
                    throw new ArgumentException();
            }
        }
    }
}
