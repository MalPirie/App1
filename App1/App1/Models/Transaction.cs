using System;

namespace App1.Models
{
    public class Transaction : IComparable<Transaction>
    {
        internal Transaction()
        { }

        public decimal Amount { get; internal set; }
        public decimal Balance { get; internal set; }
        public string Description { get; internal set; }
        public int Id { get; internal set; }
        public DateTime Timestamp { get; internal set; }

        public override bool Equals(object other)
        {
            return CompareTo(other as Transaction) == 0;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int) 2166136261;
                hash = (hash * 16777619) ^ Timestamp.GetHashCode();
                hash = (hash * 16777619) ^ Id;
                return hash;
            }
        }

        public int CompareTo(Transaction other)
        {
            if (other == null)
            {
                return 1;
            }
            int comparison = Timestamp.CompareTo(other.Timestamp);
            if (comparison == 0)
            {
                comparison = Id.CompareTo(other.Id);
            }
            return comparison;
        }

        public override string ToString() => 
            $"Transaction({Id}, Timestamp={Timestamp.ToString("yyyy-MM-dd")}, Description=\"{Description}\", Amount={Amount:0.00}, Balance={Balance:0.00})";
    }
}