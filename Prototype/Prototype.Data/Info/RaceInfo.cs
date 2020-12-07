using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype.Data
{
    public class RaceInfoBase
    {
        public String RedisKey { get; set; }
        public String RaceName { get; set; }
        public RaceInfoBase() { }
        public RaceInfoBase(RaceInfoBase r) {
            this.RaceName = r.RaceName;
            this.RedisKey = r.RedisKey;
        }
    }

    public class RaceInfo : RaceInfoBase
    {
        public new RedisKey RedisKey { get; set; }
        public RaceInfo() { }
        public RaceInfo(RaceInfoBase r) : base(r) {
            this.RedisKey = new RedisKey(r.RedisKey);
        }
    }
}
