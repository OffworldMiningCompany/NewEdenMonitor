using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace NewEdenMonitor.Model
{
    public class SavedCharacter
    {
        public long CharacterId { get; set; }
        public string Name { get; set; }
        public int KeyId { get; set; }
        public string VerificationCode { get; set; }
    }

    internal class SavedCharacterHandler
    {
        private readonly EveContext _db;

        internal SavedCharacterHandler(EveContext db)
        {
            _db = db;
        }

        internal SavedCharacter Get(int characterId)
        {
            SavedCharacter savedCharacter = null;

            using (var command = _db.Connection.CreateCommand())
            {
                command.CommandText = "SELECT character_id, name, key_id, verification_code FROM characters WHERE character_id = @character_id;";
                var param = command.Parameters.Add("character_id", DbType.Int32);
                param.Value = characterId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        savedCharacter = ReadObject(reader);
                    }

                    reader.Close();
                }
            }

            return savedCharacter;
        }

        internal async Task<SavedCharacter> GetAsync(int characterId)
        {
            return await Task.Run(() => Get(characterId));
        }

        internal List<SavedCharacter> GetAll()
        {
            var savedCharacters = new List<SavedCharacter>();

            using (var command = _db.Connection.CreateCommand())
            {
                command.CommandText = "SELECT character_id, name, key_id, verification_code FROM characters;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var savedCharacter = ReadObject(reader);
                        savedCharacters.Add(savedCharacter);
                    }

                    reader.Close();
                }
            }

            return savedCharacters;
        }

        internal async Task<List<SavedCharacter>> GetAllAsync()
        {
            return await Task.Run(() => GetAll());
        }

        internal bool Set(SavedCharacter savedCharacter)
        {
            using (var command = _db.Connection.CreateCommand())
            {
                command.CommandText = "INSERT OR REPLACE INTO characters VALUES (@character_id, @name, @key_id, @verification_code);";

                var param = command.Parameters.Add("character_id", DbType.Int64);
                param.Value = savedCharacter.CharacterId;

                param = command.Parameters.Add("name", DbType.String);
                param.Value = savedCharacter.Name;

                param = command.Parameters.Add("key_id", DbType.Int32);
                param.Value = savedCharacter.KeyId;

                param = command.Parameters.Add("verification_code", DbType.String);
                param.Value = savedCharacter.VerificationCode;

                int res = command.ExecuteNonQuery();

                if (res == 1)
                {
                    return true;
                }
            }

            return false;
        }

        internal async Task<bool> SetAsync(SavedCharacter savedCharacter)
        {
            return await Task.Run(() => Set(savedCharacter));
        }

        private SavedCharacter ReadObject(SQLiteDataReader reader)
        {
            var savedCharacter = new SavedCharacter();

            savedCharacter.CharacterId = reader["character_id"].ToInt64();
            savedCharacter.Name = reader["name"].ToString();
            savedCharacter.KeyId = reader["key_id"].ToInt32();
            savedCharacter.VerificationCode = reader["verification_code"].ToString();

            return savedCharacter;
        }
    }
}
