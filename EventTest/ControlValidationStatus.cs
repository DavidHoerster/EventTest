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
        public Int64 Timestamp;
    }


    //public class ControlValidationStatus
    //{
    //    public String ControlId;
    //    public Boolean State;
    //    private IDictionary<String, IList<ValidationStatus>> _status;

    //    public ControlValidationStatus(String id)
    //    {
    //        ControlId = id;
    //        _status = new Dictionary<String, IList<ValidationStatus>>();
    //    }

    //    public void AddStatus(ValidationStatus status)
    //    {
    //        if (_status.ContainsKey(status.Name))
    //        {
    //            _status[status.Name].Add(status);
    //        }
    //        else
    //        {
    //            _status.Add(status.Name, new List<ValidationStatus>());
    //            _status[status.Name].Add(status);
    //        }
    //    }
    //}


    //public class ValidationStatus
    //{
    //    public String Name;
    //    public String Message;
    //    public Int64 Timestamp;
    //    public Boolean Passed;
    //}
}
