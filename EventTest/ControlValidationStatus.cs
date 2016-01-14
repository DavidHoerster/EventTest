using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTest
{
    public class ControlValidatorStatus
    {
        public String ControlId;
        public String Validator;
        public Boolean State;
        public String Message;
        public Object[] Values;
        public Int64 Timestamp;
    }
    //public class ControlValidationStatus
    //{
    //    public readonly String ControlId;

    //    private IList<ValidationStatus> _status;
    //    public ControlValidationStatus(String controlId)
    //    {
    //        ControlId = controlId;
    //    }

    //    public void AddStatus(ValidationStatus status) { _status.Add(status); }
    //    public IEnumerable<ValidationStatus> GetStatus() { return _status.AsEnumerable(); }
    //}

    //public class ValidationStatus
    //{
    //    public readonly String Message;
    //    public readonly Boolean Passed;
    //    public readonly Int64 Timestamp;
    //    public ValidationStatus(Boolean isPassed, String msg, Int64 stamp) { Message = msg; Passed = isPassed; Timestamp = stamp; }
    //}
}
