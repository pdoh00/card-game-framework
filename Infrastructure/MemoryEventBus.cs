using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class MemoryEventBus : IEventBus, IEventHandlerRegistry
    {
        private List<IEventHandler> handlers = new List<IEventHandler>();
        private List<IEvent> events = new List<IEvent>();

        public MemoryEventBus(params IEventHandler[] handlers)
        {
            this.handlers.AddRange(handlers);
        }

        public void Register(IEventHandler handler)
        {
            this.handlers.Add(handler);
        }

        public void Publish(IEvent @event)
        {
            this.events.Add(@event);

            Task.Factory.StartNew(() =>
            {
                var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());

                foreach (dynamic handler in this.handlers
                    .Where(x => handlerType.IsAssignableFrom(x.GetType())))
                {
                    handler.Handle((dynamic)@event);
                }
            });
        }

        public void Publish(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                this.Publish(@event);
            }
        }

        public IEnumerable<IEvent> Events { get { return this.events; } }
    }
}
