using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using App1.Models.Events;

namespace App1.Models
{
    public class Account : Aggregate<AccountEvent>, IEquatable<Account>
    {
        private int _nextTransactionId = 0;
        private List<Transaction> _transactions = new List<Transaction>();

        public Account()
        {
            Id = Guid.NewGuid();
        }

        public Account(Guid id, IEnumerable<AccountEvent> events)
        {
            Id = id;
            ApplyEvents(events);
        }

        public decimal Balance => _transactions.LastOrDefault()?.Balance ?? 0.00m;
        public string Description { get; private set; }
        public Guid Id { get; private set; }
        public ReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        public int CreateTransaction(DateTime timestamp, string description, decimal amount)
        {
            int transactionId = _nextTransactionId++;
            Apply(new TransactionCreated()
            {
                Id = transactionId,
                Timestamp = timestamp,
                Description = description ?? $"Transaction {transactionId:00000}",
                Amount = amount
            });
            return transactionId;
        }

        public void DeleteTransaction(Transaction transaction)
        {
            if (_transactions.Contains(transaction))
            {
                Apply(new TransactionDeleted() { Id = transaction.Id });
            }
        }

        public void UpdateDescription(string description)
        {
            if (Description != description)
            {
                Apply(new AccountDescriptionUpdated() { Id = Id, Description = description});
            }
        }

        public void UpdateTransaction(
            Transaction transaction, 
            DateTime? timestamp = null,
            string description = null, 
            decimal? amount = null)
        {
            if (_transactions.Contains(transaction))
            {
                Apply(new TransactionUpdated()
                {
                    Id = transaction.Id,
                    Timestamp = timestamp,
                    Description = description,
                    Amount = amount
                });
            }
        }

        public bool Equals(Account other) => other != null && Id.Equals(other.Id);

        public override bool Equals(object other) => Equals(other as Account);

        public override int GetHashCode() => Id.GetHashCode();

        public override string ToString() => $"Account({Id}, Description=\"{Description}\", Balance={Balance:0.00})";

        private void Rebalance(int index, int count = -1)
        {

            decimal balance = (index == 0 ? 0 : _transactions[index - 1].Balance);
            int endIndex = (count == -1 ? _transactions.Count : index + count);
            while (index < endIndex)
            {
                balance += _transactions[index].Amount;
                _transactions[index].Balance = balance;
                index++;
            }
        }

        private void When(AccountDescriptionUpdated e)
        {
            Description = e.Description;
        }

        private void When(TransactionCreated e)
        { 
            var transaction = new Transaction()
            {
                Id = e.Id,
                Timestamp = e.Timestamp,
                Description = e.Description,
                Amount = e.Amount
            };
            int index = _transactions.BinarySearch(transaction);
            _transactions.Insert(~index, transaction);
            Rebalance(~index);
        }

        private void When(TransactionDeleted e)
        {
            var index = _transactions.FindIndex(t => t.Id == e.Id);
            _transactions.RemoveAt(index);
            Rebalance(index);
        }

        // The rebalancing could occur twice.
        private void When(TransactionUpdated e)
        {
            var index = _transactions.FindIndex(t => t.Id == e.Id);
            if (e.Timestamp.HasValue)
            {
                var transaction = _transactions[index];
                _transactions.RemoveAt(index);
                transaction.Timestamp = e.Timestamp.Value;
                int newIndex = _transactions.BinarySearch(transaction);
                _transactions.Insert(~newIndex, transaction);
                Rebalance(Math.Min(index, ~newIndex));
            }
            if (e.Description != null)
            {
                _transactions[index].Description = e.Description;
            }
            if (e.Amount.HasValue)
            {
                decimal previousAmount = _transactions[index].Amount;
                _transactions[index].Amount = e.Amount.Value;
                Rebalance(index);
            }
        }
    }
}