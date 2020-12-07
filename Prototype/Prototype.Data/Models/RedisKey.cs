using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype.Data
{
    public class RedisKey
    {
        public ElectionTypeEnum ElectionType { get; set; }
        public RaceTypeEnum RaceType { get; set; }
        public PartyTypeEnum PartyType { get; set; } = PartyTypeEnum.NotAPartyRace;
        public CountyTypeEnum CountyType { get; set; } = CountyTypeEnum.NotACounty;
        public int DistrictId { get; set; } = 0;
        public int Division { get; set; } = 0;
        public int Seat { get; set; } = 0;

        public RedisKey() { }
        public RedisKey(RedisKey key)
        {
            ElectionType    = key.ElectionType;
            RaceType        = key.RaceType;
            CountyType      = key.CountyType;
            PartyType       = key.PartyType;
            DistrictId      = key.DistrictId;
            Division        = key.Division;
            Seat            = key.Seat;
        }
        public RedisKey(String RedisKey)
        {
            ElectionType = (ElectionTypeEnum)Enum.Parse(typeof(ElectionTypeEnum), RedisKey.Substring(0, 2));
            RaceType = (RaceTypeEnum)Enum.Parse(typeof(RaceTypeEnum), RedisKey.Substring(2, 2));
            PartyType = (PartyTypeEnum)Enum.Parse(typeof(PartyTypeEnum), RedisKey.Substring(4, 2));
            CountyType = (CountyTypeEnum)Enum.Parse(typeof(CountyTypeEnum), RedisKey.Substring(6, 2));
            DistrictId = int.Parse(RedisKey.Substring(8, 2));
            Division = int.Parse(RedisKey.Substring(10, 2));
            Seat = int.Parse(RedisKey.Substring(12, 2));
        }

        public RedisKey(ContestId r) : this(r, CountyTypeEnum.NotACounty) { }
        public RedisKey(ContestId r, CountyTypeEnum c)
        {
            this.ElectionType = r.ElectionType;
            this.RaceType = r.RaceType;
            this.PartyType = r.PartyType;
            this.CountyType = c;
            this.DistrictId = r.DistrictId;
            this.Division = r.Division;
            this.Seat = r.Seat;
        }

        public String ToKey()
        {
            return
                (((int)ElectionType < 10) ? "0" + (int)ElectionType : ((int)ElectionType).ToString()) +
                (((int)RaceType < 10) ? "0" + (int)RaceType : ((int)RaceType).ToString()) +
                (((int)PartyType < 10) ? "0" + (int)PartyType : ((int)PartyType).ToString()) +
                (((int)CountyType < 10) ? "0" + (int)CountyType : ((int)CountyType).ToString()) +
                ((DistrictId < 10) ? "0" + DistrictId : DistrictId.ToString()) +
                ((Division < 10) ? "0" + Division : Division.ToString()) +
                ((Seat < 10) ? "0" + Seat : Seat.ToString());
        }
    }
}
