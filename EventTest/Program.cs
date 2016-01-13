using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTest
{
    class Program
    {
        static void Main(string[] args)
        {

            String rei = "REI100", formId = "FORMADV",
                   ctlA = "A", ctlB = "B", ctlC = "C";

            var instance = new ReportingEntityInstance(rei, formId);

            instance.PassValidation(ctlA, "OK", "REQUIRED");
            instance.FailValidation(ctlB, "TOO LONG", "MAXLENGTH");
            instance.PassValidation(ctlB, "OK", "PATTERN");
            instance.FailValidation(ctlC, "WRONG PATTERN", "PATTERN");

            instance.PassValidation(ctlB, "OK", "MAXLENGTH");
            instance.FailValidation(ctlB, "WRONG PATTERN", "PATTERN");

            instance.PassValidation(ctlC, "OK", "PATTERN");

            instance.PassValidation(ctlB, "OK", "PATTERN");

            instance.MarkChangesAsCommitted();
        }
    }
}
