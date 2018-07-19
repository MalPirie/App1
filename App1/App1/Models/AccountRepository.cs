using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App1.Models.Events;

namespace App1.Models
{
    public class AccountRepository
    {
        private readonly string _path;
        private List<Account> _accounts;

        public AccountRepository(string path = null)
        {
            _path = path ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Data");
            Directory.CreateDirectory(_path);
            LoadAccounts();
        }

        public Guid CreateAccount(string description)
        {
            var account = new Account();
            account.UpdateDescription(description);
            SaveAccount(account);
            _accounts.Add(account);
            return account.Id;
        }

        public void DeleteAccount(Account account)
        {
            if (!_accounts.Remove(account))
            {
                return;
            }
            var filename = MakeFilename(account.Id);
            File.Move(filename, filename + ".delete");
        }

        public Account GetAccountById(Guid id)
        {
            return _accounts.Find(a => a.Id == id);
        }

        public IEnumerable<Account> GetAllAccounts() => _accounts.AsReadOnly();

        public void SaveAccount(Account account)
        {
            var events = account.UncommittedEvents.ToArray();
            var lines = events.Select(e => e.ToData());
            File.AppendAllLines(MakeFilename(account.Id), lines);
            account.ClearUncommittedEvents();
        }

        private Account LoadAccount(Guid id)
        {
            var lines = File.ReadAllLines(MakeFilename(id));
            var events = lines.Select(Parse);
            return new Account(id, events);
        }

        private void LoadAccounts()
        {
            _accounts = new List<Account>();
            foreach (var file in Directory.EnumerateFiles(_path))
            {
                if (Guid.TryParse(Path.GetFileName(file), out var id))
                {
                    _accounts.Add(LoadAccount(id));
                }
            }
        }

        private string MakeFilename(Guid id)
        {
            return Path.Combine(_path, id.ToString("N"));
        }

        private static AccountEvent Parse(string line)
        {
            var prefixTypeMap = new Dictionary<string, Type>()
            {
                { "AU", typeof(AccountDescriptionUpdated) },
                { "TC", typeof(TransactionCreated) },
                { "TD", typeof(TransactionDeleted) },
                { "TU", typeof(TransactionUpdated) }
            };

            var prefix = line.Substring(0, 2);
            var type = prefixTypeMap[prefix];
            var accountEvent = (AccountEvent) Activator.CreateInstance(type, true);
            accountEvent.Parse(line);
            return accountEvent;
        }
    }
}