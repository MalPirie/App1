using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using App1.Models;
using App1.Utilities;

namespace App1.PageModels
{
    public class AccountListPageModel : BasePageModel
    {
        private readonly AccountRepository _repository;

        public AccountListPageModel(AccountRepository repository)
        {
            _repository = repository;
            CreateAccountCommand = new DelegateCommand(parm => Navigation.Push(new AccountDetailsPageModel(_repository, null)));
        }

        private ObservableCollection<Account> _accounts;
        public ObservableCollection<Account> Accounts
        {
            get => _accounts;
            set => SetProperty(ref _accounts, value);
        }

        public Account SelectedAccount
        {
            get => null;
            set
            {
                if (value != null)
                {
                    Navigation.Push(new TransactionListPageModel(_repository, value));
                    OnPropertyChanged();
                }
            }
        }

        public ICommand CreateAccountCommand { get; private set; }

        public override void OnPageAppearing(object sender, EventArgs e)
        {
            base.OnPageAppearing(sender, e);
            Accounts = new ObservableCollection<Account>(_repository.GetAllAccounts());
        }
    }
}