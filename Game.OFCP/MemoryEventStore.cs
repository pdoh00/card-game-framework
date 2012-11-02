using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Game.OFCP
{
    public class MemoryEventStore : IEventStore
    {
        Dictionary<string, List<EventDescriptor>> _store = new Dictionary<string, List<EventDescriptor>>();
        private readonly IEventBus _eventBus;
        private struct EventDescriptor
        {

            public readonly IEvent EventData;
            public readonly string AggregateId;
            public readonly int Version;

            public EventDescriptor(string aggregateId, IEvent eventData, int version)
            {
                EventData = eventData;
                Version = version;
                AggregateId = aggregateId;
            }
        }

        public MemoryEventStore(IEventBus bus)
        {
            _eventBus = bus;
        }

        public void SaveEvents(AggregateRoot eventSourced, int expectedVersion)
        {
            List<EventDescriptor> eventDescriptors;
            if (!_store.TryGetValue(eventSourced.Id, out eventDescriptors))
            {
                eventDescriptors = new List<EventDescriptor>();
                _store.Add(eventSourced.Id, eventDescriptors);
            }

            else if (eventDescriptors.Last().Version != expectedVersion && expectedVersion != -1)
                throw new ConcurrencyException(String.Format("The aggregate has changed since you started. Expected version {0}, Current Version {1}", expectedVersion, eventDescriptors.Last().Version));


            var i = expectedVersion;
            foreach (var @event in eventSourced.GetUncommitedChanges())
            {
                i++;
                @event.Version = i;
                eventDescriptors.Add(new EventDescriptor(eventSourced.Id, @event, i));
                //Now that the event has actually been stored, let everyone else know.
                _eventBus.Publish(@event);
            }
        }

        public List<IEvent> GetEventsForAggregate(string aggregateId)
        {
            List<EventDescriptor> eventDescriptors;
            if (!_store.TryGetValue(aggregateId, out eventDescriptors))
            {
                throw new AggregateNotFoundException(String.Format("Cannot find aggregate with id {0}", aggregateId));
            }
            return eventDescriptors.Select(desc => desc.EventData).ToList();
        }
    }
}
