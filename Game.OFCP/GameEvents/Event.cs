using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Game.OFCP.Events
{
    public abstract class Event : IEvent
    {
        private Guid _eventId = Guid.NewGuid();
        private DateTime _eventTimestamp = DateTime.Now.ToUniversalTime();
        private int _version = -1;
        public Guid EventId
        {
            get { return _eventId; }
        }

        public DateTime EventTimestamp
        {
            get { return _eventTimestamp; }
        }

        public int Version
        {
            get { return _version; }
            set { _version = value; }
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((Event)obj);
        }
        public bool Equals(Event other)
        {
            return EventId.Equals(other.EventId);
        }
        // override object.GetHashCode
        public override int GetHashCode()
        {
            return EventId.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{0}: ", EventTimestamp.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
        }
    }
}
