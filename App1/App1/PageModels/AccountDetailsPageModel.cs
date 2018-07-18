using System;
using System.Windows.Input;
using App1.Models;
using App1.Utilities;

namespace App1.PageModels
{
    public class AccountDetailsPageModel : BasePageModel
    {
        private readonly AccountRepository _repository = new AccountRepository();
        private readonly Account _account;
        private readonly bool _isNewAccount;

        public AccountDetailsPageModel(Account account)
        {
            _account = account;
            _isNewAccount = (account == null);
            if (_isNewAccount)
            {
                _description = "New account";
            }

            SaveAccountCommand = new DelegateCommand(parm =>
            {
                if (_isNewAccount)
                {
                    Guid id = _repository.CreateAccount(_description);
                    Push(new TransactionListPageModel(_repository.GetAccountById(id)));
                    Remove();
                }
                else
                {
                    _account.UpdateDescription(_description);
                    _repository.SaveAccount(_account);
                    Pop();
                }
            });
        }

        public string _description;
        public string Description
        {
            get => _description ?? _account.Description;
            set
            {
                if (Description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveAccountCommand { get; set; }
    }
}