//https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/listview/interactivity#Context_Actions
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using App1.Models;
using App1.Services;
using App1.Utilities;

namespace App1.PageModels
{
    public class TransactionListPageModel : BasePageModel
    {
        private readonly AccountRepository _repository;
        private readonly Account _account;

        public TransactionListPageModel(AccountRepository repository, Account account)
        {
            _repository = repository;
            _account = account;
            _repository = new AccountRepository();
            var dialog = new DialogService();

            CreateTransactionCommand = new DelegateCommand(parm => Navigation.Push(new TransactionDetailsPageModel(_repository, _account, null)));
            DeleteAccountCommand = new DelegateCommand(async parm =>
            {
                var result = await dialog.ShowMessage("Delete Account", "Are you sure?", "Yes", "No");
                if (result)
                {
                    _repository.DeleteAccount(_account);
                    Navigation.Pop();
                }
            });
            DeleteTransactionCommand = new DelegateCommand(async parm => 
            {
                if (parm is Transaction transaction)
                {
                    var result = await dialog.ShowMessage("Delete Transaction", "Are you sure?", "Yes", "No");
                    if (result)
                    {
                        _account.DeleteTransaction(transaction);
                        _repository.SaveAccount(_account);
                    }
                }
            });
            EditAccountCommand = new DelegateCommand(parm => Navigation.Push(new AccountDetailsPageModel(_repository, _account)));
            EditTransactionCommand = new DelegateCommand(parm =>
            {
                if (parm is Transaction transaction)
                {
                    Navigation.Push(new TransactionDetailsPageModel(_repository, _account, transaction));
                }
            });
        }

        public Account Account => _account;

        public Transaction SelectedTransaction
        {
            get => null;
            set
            {
                if (value != null)
                {
                    Navigation.Push(new TransactionDetailsPageModel(_repository, _account, value));
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<Transaction> _transactions;
        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            set => SetProperty(ref _transactions, value);
        }

        public ICommand CreateTransactionCommand { get; private set; }
        public ICommand DeleteAccountCommand { get; private set; }
        public ICommand DeleteTransactionCommand { get; private set; }
        public ICommand EditAccountCommand { get; private set; }
        public ICommand EditTransactionCommand { get; private set; }

        public override void OnPageAppearing(object sender, EventArgs e)
        {
            base.OnPageAppearing(sender, e);
            Transactions = new ObservableCollection<Transaction>(_account.Transactions);
        }
    }
}