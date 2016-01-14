using EventTest.Events;
using Cti.Platform.Events;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

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
            var coll = SetupMongoMaps();

            var timestamp = DateTime.UtcNow.Ticks;

            PrintInstructions();

            Console.WriteLine("Form ID: ");
            var formId = Console.ReadLine();
            Console.WriteLine("Reporting Entity Instance ID: ");
            var rei = Console.ReadLine();

            ReportingEntityInstance instance = LoadDomain(coll, formId, rei);

            while (true)
            {
                var cmd = Console.ReadLine();
                var cmdParts = cmd.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                switch (cmdParts[0])
                {
                    case "Q":
                        Console.WriteLine("BYE!!");
                        return;
                        break;
                    case "L":
                        formId = cmdParts[1];
                        rei = cmdParts[2];
                        instance = LoadDomain(coll, formId, rei);
                        Console.WriteLine($"Form ID {formId} REI ID {rei} loaded!!");
                        break;
                    case "F":
                        var failures = instance.GetFailingControls();
                        Console.WriteLine("Current failing controls...");
                        foreach (var fail in failures)
                        {
                            Console.WriteLine($"{fail.ControlId} {fail.State} {fail.Validator} {fail.Message}");
                        }
                        Console.WriteLine("");
                        break;

                    case "S":
                        Console.WriteLine("Current status for all controls...");

                        var status = instance.GetStatus();
                        foreach (var stat in status)
                        {
                            Console.WriteLine($"{stat.ControlId} {stat.State} {stat.Validator} {stat.Message}");
                        }
                        Console.WriteLine("");
                        break;
                    case "P":
                        PersistEvents(coll, instance);

                        timestamp = DateTime.UtcNow.Ticks;

                        Console.WriteLine("PERSISTED!");
                        Console.WriteLine("");
                        break;
                    case "C":
                        PrintInstructions();
                        break;
                    case "ADD":
                        var isPass = cmdParts[1].Equals("PASS");
                        if (isPass)
                        {
                            instance.PassValidation(cmdParts[2], cmdParts[3], cmdParts[4], timestamp, cmdParts[5]);
                        }
                        else
                        {
                            instance.FailValidation(cmdParts[2], cmdParts[3], cmdParts[4], timestamp, cmdParts[5]);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private static void PersistEvents(IMongoCollection<ValidationEventBase> coll, ReportingEntityInstance instance)
        {
            var validations = instance.GetUncommittedChanges();
            coll.InsertMany(validations.OfType<ValidationEventBase>());
            instance.MarkChangesAsCommitted();
        }

        private static ReportingEntityInstance LoadDomain(IMongoCollection<ValidationEventBase> coll, string formId, string rei)
        {
            var instance = new ReportingEntityInstance(formId, rei);
            var events = coll.Find<ValidationEventBase>(veb => veb.FormId == formId && veb.ReportingEntityInstanceId == rei)
                             .SortBy(veb => veb.Timestamp);
            instance.LoadFromHistory(events.ToEnumerable());
            return instance;
        }

        private static void PrintInstructions()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine(" ADD PASS|FAIL <CTRL_ID> <VALIDATION> <MSG> <VAL1> <VAL2> ...");
            Console.WriteLine(" [L]OAD <FORMID> <REI>");
            Console.WriteLine(" [P]ERSIST");
            Console.WriteLine(" [S]TATUS");
            Console.WriteLine(" [F]AILURES");
            Console.WriteLine(" [C]OMMANDS");
            Console.WriteLine(" [Q]UIT");

            Console.WriteLine("");
        }

        private static IMongoCollection<ValidationEventBase> SetupMongoMaps()
        {
            BsonClassMap.RegisterClassMap<EventBase>(cm =>
            {
                cm.AutoMap();
                cm.AddKnownType(typeof(ValidationEventBase));
                cm.AddKnownType(typeof(ValidationPassed));
                cm.AddKnownType(typeof(ValidationFailed));

                cm.SetIsRootClass(true);
            });
            BsonClassMap.RegisterClassMap<ValidationEventBase>(cm =>
            {
                cm.AutoMap();
                cm.AddKnownType(typeof(ValidationPassed));
                cm.AddKnownType(typeof(ValidationFailed));

                cm.SetIsRootClass(true);
            });

            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("Events");
            var coll = db.GetCollection<ValidationEventBase>("Validations");

            return coll;
        }
    }
}
