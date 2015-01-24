using System.Threading.Tasks;
using eZet.EveLib.EveXmlModule;
using eZet.EveLib.EveXmlModule.Models.Misc;
using eZet.EveLib.EveXmlModule.RequestHandlers;
using NewEdenMonitor.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;

namespace NewEdenMonitor.Data
{
    internal class CharacterInfoUpdater : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static readonly Dictionary<int, CharacterInfoUpdater> Instances = new Dictionary<int, CharacterInfoUpdater>();
        private static readonly object SyncRoot = new Object();

        private readonly Timer _timer;
        private readonly Character _character;
        private CharacterInfo _characterInfo;

        public CharacterInfoUpdater(CharacterKey characterKey, long characterId)
        {
            if (characterKey == null)
            {
                throw new ArgumentNullException("characterKey", "CharacterKey must not be null.");
            }

            _character = characterKey.Characters.FirstOrDefault(c => c.CharacterId == characterId);

            if (_character == null)
            {
                throw new ArgumentException("Character not found.");
            }

            ((EveXmlRequestHandler)_character.RequestHandler).Cache = SqliteCache.Instance;

            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += Tick;

            _characterInfo = new CharacterInfo();

            Task.Factory.StartNew(Update);
        }

        public static CharacterInfoUpdater Instance(CharacterKey characterKey, long characterId)
        {
            int hashCode = 37;
            hashCode += characterKey.GetHashCode();
            hashCode *= 397;
            hashCode += characterId.GetHashCode();

            if (!Instances.ContainsKey(hashCode))
            {
                lock (SyncRoot)
                {
                    if (!Instances.ContainsKey(hashCode))
                    {
                        var characterInfoUpdater = new CharacterInfoUpdater(characterKey, characterId);
                        Instances.Add(hashCode, characterInfoUpdater);
                        return characterInfoUpdater;
                    }

                    return Instances[hashCode];
                }
            }

            lock (SyncRoot)
            {
                return Instances[hashCode];
            }
        }

        public CharacterInfo CharacterInfo
        {
            get { return _characterInfo; }
            set
            {
                _characterInfo = value;
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
            var characterInfo = _character.GetCharacterInfo();

            if (characterInfo.CachedUntil > DateTime.UtcNow)
            {
                _timer.Interval = (characterInfo.CachedUntil - DateTime.UtcNow).TotalMilliseconds;
            }
            else
            {
                _timer.Interval = 10 * 60 * 1000;
            }

            _timer.Start();

            lock (CharacterInfo)
            {
                CharacterInfo = characterInfo.Result;
            }
        }
    }
}
