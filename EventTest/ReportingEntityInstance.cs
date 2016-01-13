using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventTest.Events;

namespace EventTest
{
    public class ReportingEntityInstance : AggregateRoot
    {
        public readonly String FormDefinitionId, Id;

        private IDictionary<String, ControlValidatorStatus> _status;

        public ReportingEntityInstance(String id, String formId)
        {
            Id = id;
            FormDefinitionId = formId;
            _status = new Dictionary<String, ControlValidatorStatus>();
        }

        public void PassValidation(String controlId, String message, String validator)
        {
            ApplyChange(new ValidationPassed
            {
                ControlId = controlId,
                Message = message,
                Validator = validator,
                Timestamp = DateTime.UtcNow.Ticks
            });
        }

        public void FailValidation(String controlId, String message, String validator)
        {
            ApplyChange(new ValidationFailed
            {
                ControlId = controlId,
                Message = message,
                Validator = validator,
                Timestamp = DateTime.UtcNow.Ticks
            });
        }

        private void Apply(ValidationPassed evt)
        {
            var key = $"{evt.ControlId}-{evt.Validator}";
            if (_status.ContainsKey(key))
            {
                var item = _status[key];
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
                    Validator = evt.Validator
                };
                _status.Add(key, item);
            }
        }

        private void Apply(ValidationFailed evt)
        {
            var key = $"{evt.ControlId}-{evt.Validator}";
            if (_status.ContainsKey(key))
            {
                var item = _status[key];
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
                    Validator = evt.Validator
                };
                _status.Add(key, item);
            }
        }

    }
}
