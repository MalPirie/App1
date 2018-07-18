using System;

namespace App1.Models.Events
{
    public class TransactionDeleted : AccountEvent
    {
        public int Id { get; set; }

        public override void Parse(string line)
        {
            var fields = line.Split(' ');
            Id = int.Parse(fields[1]);
        }

        public override string ToData() => $"TD {Id}";
    }
}