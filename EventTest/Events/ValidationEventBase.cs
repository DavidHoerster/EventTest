using Cti.Platform.Events;
using System;

namespace EventTest.Events
{
    public abstract class ReiEventBase : EventBase
    {
        public String FormId { get; set; }
        public String ReportingEntityInstanceId { get; set; }
        public Int64 Timestamp { get; set; }
        public DateTime Date { get; set; }
    }

    public abstract class ValidationEventBase : ReiEventBase
    {
        public String ControlId { get; set; }
        public String Validator { get; set; }
        public String Message { get; set; }
        public Object[] Values { get; set; }
    }
}