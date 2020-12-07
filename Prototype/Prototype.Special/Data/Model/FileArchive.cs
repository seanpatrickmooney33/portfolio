using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpecialElection.Data.Model
{
    public enum FileArchiveType { Rmsg, Cmsg, Pmsg, Smsg, Vmsg }

    public class FileArchive
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public FileArchiveType FileArchiveType { get; set; }
        public DateTime RowCreateDate { get; set; } = DateTime.UtcNow;
        public byte[] FileData { get; set; }
    }
}
