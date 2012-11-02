using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IEventBus
    {
        void Publish(IEvent @event);
        void Publish(IEnumerable<IEvent> events);
    }
}
