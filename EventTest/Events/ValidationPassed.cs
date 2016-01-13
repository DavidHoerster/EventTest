using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTest.Events
{
    public class ValidationPassed : EventBase
    {
        public String ControlId { get; set; }
        public String Message { get; set; }
        public String Validator { get; set; }
        public Int64 Timestamp { get; set; }
    }
}
