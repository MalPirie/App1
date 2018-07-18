using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using App1.Utilities;

namespace App1.Models
{
    public abstract class Aggregate<T>
    {
        private List<T> _uncommittedEvents = new List<T>();
        public IEnumerable<T> UncommittedEvents => _uncommittedEvents;

        protected void ApplyEvents(IEnumerable<T> events)
        {
            foreach (var e in events)
            {
                Apply(e, false);
            }
        }

        public void ClearUncommittedEvents() 
        {
            _uncommittedEvents.Clear();
        }

        protected void Apply(T e)
        {
            Apply(e, true);
        }

        private void Apply(T e, bool commit)
        {
            RedirectToWhen.InvokeEventOptional(this, e);
            if (commit) 
            {
                _uncommittedEvents.Add(e);
            }
        }
    }
}