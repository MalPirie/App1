//https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/listview/interactivity#Context_Actions
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using App1.Models;
using App1.Services;
using App1.Utilities;

namespace App1.PageModels
{
    public class TransactionListPageModel : BasePageModel
    {
        private readonly DialogService _dialog;
        private readonly AccountRepository _repository;
        private readonly Account _account;

        public TransactionListPageModel(Account account)
        {
            _account = account;
            _dialog = new DialogService();
            _repository = new AccountRepository();
            
            CreateTransactionCommand = new DelegateCommand(parm => Push(new TransactionDetailsPageModel(_account, null)));
            DeleteAccountCommand = new DelegateCommand(parm =>
            {
                if (_dialog.ShowMessage("Delete Account", "Are you sure?", "Yes", "No"))
                {
                    _repository.DeleteAccount(_account);
                    Pop();
                }
            });
            DeleteTransactionCommand = new DelegateCommand(parm => 
            {
                var transaction = parm as Transaction;
                if (transaction != null)
                {
                    if (_dialog.ShowMessage("Delete Transaction", "Are you sure?", "Yes", "No"))
                    {
                        _account.DeleteTransaction(transaction);
                        _repository.SaveAccount(_account);
                    }
                }
            });
            EditAccountCommand = new DelegateCommand(parm => Push(new AccountDetailsPageModel(_account)));
            EditTransactionCommand = new DelegateCommand(parm =>
            {
                var transaction = parm as Transaction;
                if (transaction != null)
                {
                    Push(new TransactionDetailsPageModel(_account, transaction));
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
                    Push(new TransactionDetailsPageModel(_account, value));
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