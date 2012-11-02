using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Game.OFCP
{
    public abstract class AggregateRoot
    {
        private const int DefaultVersion = 0;
        private readonly Dictionary<Type, Action<IEvent>> handlers = new Dictionary<Type, Action<IEvent>>();
        private List<IEvent> _uncommitedChanges = new List<IEvent>();

        public abstract string Id { get; }

        public int Version { get; internal set; }

        public AggregateRoot()
        {
            Version = DefaultVersion;
        }
        public void Apply(IEvent @event)
        {
            this.handlers[@event.GetType()].Invoke(@event);
            _uncommitedChanges.Add(@event);
        }

        public void Apply(IEnumerable<IEvent> @events)
        {
            foreach (var @event in @events)
            {
                this.handlers[@event.GetType()].Invoke(@event);
                _uncommitedChanges.Add(@event);
            }
        }

        public List<IEvent> GetUncommitedChanges()
        {
            return _uncommitedChanges;
        }

        protected void Handles<TEvent>(Action<TEvent> handler)
            where TEvent : IEvent
        {
            this.handlers.Add(typeof(TEvent), @event => handler((TEvent)@event));
        }

        public void LoadFrom(IEnumerable<IEvent> pastEvents)
        {
            foreach (var e in pastEvents)
            {
                this.handlers[e.GetType()].Invoke(e);
                this.Version = e.Version;
            }
        }
    }
}
