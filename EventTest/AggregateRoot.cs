using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventTest.Events;

namespace EventTest
{
    public abstract class AggregateRoot
    {
        private readonly List<EventBase> _changes = new List<EventBase>();

        public IEnumerable<EventBase> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadFromHistory(IEnumerable<EventBase> events)
        {
            foreach (var @event in events) ApplyChange(@event, false);
        }

        protected void ApplyChange(EventBase @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(EventBase @event, bool isNew)
        {
            this.AsDynamic().Apply(@event);
            if (isNew) _changes.Add(@event);
        }
    }
}
