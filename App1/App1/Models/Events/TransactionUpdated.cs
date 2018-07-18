using System;

namespace App1.Models.Events
{
    public class TransactionUpdated : AccountEvent
    {
        public int Id { get; set; }
        public decimal? Amount { get; set; }
        public string Description { get; set; }
        public DateTime? Timestamp { get; set; }

        public override void Parse(string line)
        { 
            var fields = line.Split(' ');
            Id = Int32.Parse(fields[1]);
            if (fields[2] != "_")
            {
                Timestamp = DateTime.ParseExact(fields[2], "yyyyMMdd", null);
            }
            Description = fields[3] == "_" ? null : fields[3].Replace("_", " ");
            if (fields[4] != "_")
            {
                Amount = Decimal.Parse(fields[4]);
            }
        }

        public override string ToData()
        { 
            return String.Format("TU {0} {1} {2} {3}",
                Id,
                Timestamp?.ToString("yyyyMMdd") ?? "_",
                Description?.Replace(" ", "_") ?? "_",
                Amount?.ToString() ?? "_");
        }
    }
}