using System;
using System.Windows.Input;
using App1.Models;
using App1.Utilities;

namespace App1.PageModels
{
    public class TransactionDetailsPageModel : BasePageModel
    {
        private readonly AccountRepository _repository;
        private readonly Account _account;
        private readonly Transaction _transaction;
        private readonly bool _isNewTransaction;

        public TransactionDetailsPageModel(AccountRepository repository, Account account, Transaction transaction)
        {
            _repository = repository;
            _account = account;
            _transaction = transaction;
            _isNewTransaction = (transaction == null);
            if (_isNewTransaction)
            {
                _timestamp = DateTime.UtcNow.Date;
                _description = "New transaction";
                _amount = 0.00m;
            }

            _repository = new AccountRepository();

            SaveTransactionCommand = new DelegateCommand(parm =>
            {
                if (_isNewTransaction)
                {
                    _account.CreateTransaction(_timestamp.Value, _description, _amount.Value);
                }
                else
                {
                    _account.UpdateTransaction(_transaction, _timestamp, _description, _amount);
                }
                _repository.SaveAccount(_account);
                Navigation.Pop();
            });
        }

        private decimal? _amount;
        public decimal Amount
        {
            get => _amount ?? _transaction.Amount;
            set
            {
                if (Amount != value)
                {
                    _amount = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _description;
        public string Description
        {
            get => _description ?? _transaction.Description;
            set
            {
                if (Description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime? _timestamp;
        public DateTime Timestamp
        {
            get => _timestamp ?? _transaction.Timestamp;
            set
            {
                if (Timestamp != value)
                {
                    _timestamp = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveTransactionCommand { get; private set; }
    }
}