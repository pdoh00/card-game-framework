using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IEvent
    {
        Guid EventId { get; }
        DateTime EventTimestamp { get; }
        int Version { get; set; }
    }
}
