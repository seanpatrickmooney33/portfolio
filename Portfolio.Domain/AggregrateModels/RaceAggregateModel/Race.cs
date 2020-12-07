using System;
using System.Collections.Generic;
using System.Text;
using Portfolio.Common.Foundation;

namespace Portfolio.Domain.AggregrateModels.RaceAggregateModel
{
    public class Race : IAggregateRoot
    {
        public String RaceName { get; private set; }
        public DateTime RaceDate { get; private set; }
        public RaceTypeEnum RaceType { get; private set; }

        public Race(String raceName, DateTime raceDate, RaceTypeEnum raceType)
        {
            RaceName = raceName;
            RaceDate = raceDate;
            RaceType = raceType;
        }
    }
}
