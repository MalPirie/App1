using System;

namespace App1.Models.Events
{
    public class AccountDescriptionUpdated : AccountEvent
    {
        public Guid Id { get; set; }
        public string Description { get; set; }

        public override void Parse(string line)
        {
            var fields = line.Split(' ');
            Description = fields[1].Replace("_", " ");
        }

        public override string ToData() => $"AU {Description.Replace(" ", "_")}";
    }
}