using System;

namespace App1.Models.Events
{
    public abstract class AccountEvent
    {
        public abstract void Parse(string line);
        public abstract string ToData();
    }
}