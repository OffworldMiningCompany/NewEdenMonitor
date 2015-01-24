using eZet.EveLib.EveXmlModule;
using eZet.EveLib.EveXmlModule.Models.Character;
using eZet.EveLib.EveXmlModule.RequestHandlers;
using NewEdenMonitor.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;

namespace NewEdenMonitor.Data
{
    internal class CharacterSheetUpdater : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static readonly Dictionary<int, CharacterSheetUpdater> Instances = new Dictionary<int, CharacterSheetUpdater>();
        private static readonly object SyncRoot = new Object();

        private readonly Timer _timer;
        private readonly Character _character;
        private CharacterSheet _characterSheet;

        private CharacterSheetUpdater(CharacterKey characterKey, long characterId)
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

            _characterSheet = new CharacterSheet();

            Task.Factory.StartNew(Update);
        }

        public static CharacterSheetUpdater Instance(CharacterKey characterKey, long characterId)
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
                        var characterSheetUpdater = new CharacterSheetUpdater(characterKey, characterId);
                        Instances.Add(hashCode, characterSheetUpdater);
                        return characterSheetUpdater;
                    }

                    return Instances[hashCode];
                }
            }

            lock (SyncRoot)
            {
                return Instances[hashCode];
            }
        }

        public CharacterSheet CharacterSheet
        {
            get { return _characterSheet; }
            set
            {
                _characterSheet = value;
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
            var characterSheet = _character.GetCharacterSheet();

            if (characterSheet.CachedUntil > DateTime.UtcNow)
            {
                _timer.Interval = (characterSheet.CachedUntil - DateTime.UtcNow).TotalMilliseconds;
            }
            else
            {
                _timer.Interval = 10 * 60 * 1000;
            }

            _timer.Start();

            lock (CharacterSheet)
            {
                CharacterSheet = characterSheet.Result;
            }
        }
    }
}
