using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NewEdenMonitor.Annotations;
using eZet.EveLib.EveXmlModule;
using System;
using System.Threading.Tasks;
using System.Timers;
using eZet.EveLib.EveXmlModule.Models.Account;

namespace NewEdenMonitor.Data
{
    class AccountStatusUpdater : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static readonly Dictionary<int, AccountStatusUpdater> Instances = new Dictionary<int, AccountStatusUpdater>();
        private static readonly object SyncRoot = new Object();

        private readonly Timer _timer;
        private readonly CharacterKey _characterKey;
        private AccountStatus _accountStatus;

        private AccountStatusUpdater(CharacterKey characterKey)
        {
            if (characterKey == null)
            {
                throw new ArgumentNullException("characterKey", "CharacterKey must not be null.");
            }

            _characterKey = characterKey;

            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += Tick;

            _accountStatus = new AccountStatus();

            Task.Factory.StartNew(Update);
        }

        public static AccountStatusUpdater Instance(CharacterKey characterKey)
        {
            int hashCode = characterKey.GetHashCode();

            if (!Instances.ContainsKey(hashCode))
            {
                lock (SyncRoot)
                {
                    if (!Instances.ContainsKey(hashCode))
                    {
                        var accountStatusUpdater = new AccountStatusUpdater(characterKey);
                        Instances.Add(hashCode, accountStatusUpdater);
                        return accountStatusUpdater;
                    }

                    return Instances[hashCode];
                }
            }

            lock (SyncRoot)
            {
                return Instances[hashCode];
            }
        }

        public AccountStatus AccountStatus
        {
            get { return _accountStatus; }
            set
            {
                _accountStatus = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Tick(object sender, ElapsedEventArgs args)
        {
            Update();
        }

        public void Update()
        {
            var accountStatus = _characterKey.GetAccountStatus();

            if (accountStatus.CachedUntil > DateTime.UtcNow)
            {
                _timer.Interval = (accountStatus.CachedUntil - DateTime.UtcNow).TotalMilliseconds;
            }
            else
            {
                _timer.Interval = 10 * 60 * 1000;
            }

            _timer.Start();

            lock (_accountStatus)
            {
                _accountStatus = accountStatus.Result;
            }
        }
    }
}
