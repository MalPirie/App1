using System;
using System.Windows.Input;
using App1.Models;
using App1.Utilities;

namespace App1.PageModels
{
    public class AccountDetailsPageModel : BasePageModel
    {
        private readonly AccountRepository _repository;
        private readonly Account _account;

        public AccountDetailsPageModel(AccountRepository repository, Account account)
        {
            _repository = repository;
            _account = account;
            var isNewAccount = (account == null);
            if (isNewAccount)
            {
                _description = "New account";
            }

            SaveAccountCommand = new DelegateCommand(parm =>
            {
                if (isNewAccount)
                {
                    var id = _repository.CreateAccount(_description);
                    Navigation.Push(new TransactionListPageModel(_repository, _repository.GetAccountById(id)));
                    Navigation.Remove(this);
                }
                else
                {
                    _account.UpdateDescription(_description);
                    _repository.SaveAccount(_account);
                    Navigation.Pop();
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