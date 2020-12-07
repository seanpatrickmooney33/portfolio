using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype.Data.Models.VoteResults
{
    public class VoteRecord<T>
    {
        public RedisKey Key { get; set; }
        public List<T> VoteResults { get; set; } = new List<T>();
        public VoteRecord() { }
    }
}
