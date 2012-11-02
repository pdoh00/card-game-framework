
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Game.OFCP
{
    public interface IEventStore
    {
        void SaveEvents(AggregateRoot eventSourced, int expectedVersion);
        List<IEvent> GetEventsForAggregate(string aggregateId);
    }

    [Serializable]
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException() { }
        public ConcurrencyException(string message) : base(message) { }
        public ConcurrencyException(string message, Exception inner) : base(message, inner) { }
        protected ConcurrencyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class AggregateNotFoundException : Exception
    {
        public AggregateNotFoundException() { }
        public AggregateNotFoundException(string message) : base(message) { }
        public AggregateNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected AggregateNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
