using EventTest.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTest
{
    public class ReportingEntityInstance : AggregateRoot
    {
        public readonly String FormDefinitionId, ReportingEntityId;

        private IDictionary<String, ControlValidatorStatus> _controlStatus;
        public ReportingEntityInstance(String formId, String reportingid)
        {
            FormDefinitionId = formId;
            ReportingEntityId = reportingid;
            _controlStatus = new Dictionary<String, ControlValidatorStatus>();
        }


        public void PassValidation(String controlId, String validator, String message, Int64 timestamp, params Object[] vals)
        {
            ApplyChange(new ValidationPassed
            {
                FormId = FormDefinitionId,
                ReportingEntityInstanceId = ReportingEntityId,
                Id = Guid.NewGuid(),
                ControlId = controlId,
                Date = DateTime.UtcNow,
                Message = message,
                Timestamp = timestamp,
                Validator = validator,
                Values = vals
            });
        }

        public void FailValidation(String controlId, String validator, String message, Int64 timestamp, params Object[] vals)
        {
            ApplyChange(new ValidationFailed
            {
                FormId = FormDefinitionId,
                ReportingEntityInstanceId = ReportingEntityId,
                Id = Guid.NewGuid(),
                ControlId = controlId,
                Date = DateTime.UtcNow,
                Message = message,
                Timestamp = timestamp,
                Validator = validator,
                Values = vals
            });
        }

        public IEnumerable<ControlValidatorStatus> GetFailingControls()
        {
            return _controlStatus.Where(c => c.Value.State == false)
                                    .Select(c => c.Value);
        }

        public IEnumerable<ControlValidatorStatus> GetStatus()
        {
            return _controlStatus.Select(c => c.Value);
        }

        private void Apply(ValidationPassed evt)
        {
            var key = $"{evt.ControlId}-{evt.Validator}";
            if (_controlStatus.ContainsKey(key))
            {
                var item = _controlStatus[key];
                item.Values = evt.Values;
                item.State = true;
                item.Message = evt.Message;
                item.Timestamp = evt.Timestamp;
            }
            else
            {
                var item = new ControlValidatorStatus
                {
                    ControlId = evt.ControlId,
                    Message = evt.Message,
                    State = true,
                    Timestamp = evt.Timestamp,
                    Values = evt.Values,
                    Validator = evt.Validator
                };
                _controlStatus.Add(key, item);
            }
        }

        private void Apply(ValidationFailed evt)
        {
            var key = $"{evt.ControlId}-{evt.Validator}";
            if (_controlStatus.ContainsKey(key))
            {
                var item = _controlStatus[key];
                item.Values = evt.Values;
                item.State = false;
                item.Message = evt.Message;
                item.Timestamp = evt.Timestamp;
            }
            else
            {
                var item = new ControlValidatorStatus
                {
                    ControlId = evt.ControlId,
                    Message = evt.Message,
                    State = false,
                    Timestamp = evt.Timestamp,
                    Values = evt.Values,
                    Validator = evt.Validator
                };
                _controlStatus.Add(key, item);
            }
        }
    }
}
