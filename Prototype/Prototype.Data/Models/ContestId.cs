using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype.Data
{
    public class ContestId
    {
        public RaceTypeEnum RaceType { get; set; } = 0;
        public ElectionTypeEnum ElectionType { get; set; } = 0;
        public PartyTypeEnum PartyType { get; set; } = 0;
        public int DistrictId { get; set; } = 0;
        public int Division { get; set; } = 0;
        public int Seat { get; set; } = 0;

        public ContestId() { }
        public ContestId(String OpCode) 
        {
            RaceType = (RaceTypeEnum)Enum.Parse(typeof(RaceTypeEnum),OpCode.Substring(0, 2));
            ElectionType = (ElectionTypeEnum)Enum.Parse(typeof(ElectionTypeEnum), OpCode.Substring(2, 2));
            PartyType = (PartyTypeEnum)Enum.Parse(typeof(PartyTypeEnum), OpCode.Substring(4, 2));
            DistrictId = int.Parse(OpCode.Substring(6, 2));
            Division = int.Parse(OpCode.Substring(8, 2));
            Seat = int.Parse(OpCode.Substring(10, 2));
        }

        public string ToOpCode() 
        {
            return 
                (((int)RaceType < 10) ? "0" + (int)RaceType : ((int)RaceType).ToString()) +
                (((int)ElectionType < 10) ? "0" + (int)ElectionType : ((int)ElectionType).ToString()) +
                (((int)PartyType < 10) ? "0" + (int)PartyType : ((int)PartyType).ToString()) +
                ((DistrictId < 10) ? "0" + DistrictId : DistrictId.ToString()) +
                ((Division < 10) ? "0" + Division : Division.ToString()) +
                ((Seat < 10) ? "0" + Seat : Seat.ToString());
        }
    }
}
