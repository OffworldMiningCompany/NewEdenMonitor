using System;
using System.Data;
using System.Threading.Tasks;

namespace NewEdenMonitor.Model
{
    public class SavedCharacter
    {
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public int KeyId { get; set; }
        public string VerificationCode { get; set; }
        public byte[] Portrait { get; set; }
    }

    internal class SavedCharacterHandler
    {
        private readonly EveContext _parent;

        internal SavedCharacterHandler(EveContext parent)
        {
            _parent = parent;
        }

        internal SavedCharacter Get(int characterId)
        {
            SavedCharacter savedCharacter = null;

            using (var command = _parent.Connection.CreateCommand())
            {
                command.CommandText = "SELECT character_id, name, key_id, verification_code, portrait FROM characters WHERE character_id = @character_id;";
                var param = command.Parameters.Add("character_id", DbType.Int32);
                param.Value = characterId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        savedCharacter = new SavedCharacter();

                        savedCharacter.CharacterId = (int)reader["character_id"];
                        savedCharacter.Name = reader["name"].ToString();
                        savedCharacter.KeyId = (int)reader["key_id"];
                        savedCharacter.VerificationCode = reader["verification_code"].ToString();
                        savedCharacter.Portrait = (byte[]) reader["portrait"];
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

        internal bool Add(SavedCharacter savedCharacter)
        {
            using (var command = _parent.Connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO characters VALUES (@character_id, @name, @key_id, @verification_code, @portrait);";

                var param = command.Parameters.Add("character_id", DbType.Int32);
                param.Value = savedCharacter.CharacterId;

                param = command.Parameters.Add("name", DbType.DateTime);
                param.Value = savedCharacter.Name;

                param = command.Parameters.Add("key_id", DbType.Int32);
                param.Value = savedCharacter.KeyId;

                param = command.Parameters.Add("verification_code", DbType.String);
                param.Value = savedCharacter.VerificationCode;

                param = command.Parameters.Add("portrait", DbType.Binary);
                param.Value = savedCharacter.Portrait;

                int res = command.ExecuteNonQuery();

                if (res == 1)
                {
                    return true;
                }
            }

            return false;
        }

        internal async Task<bool> AddAsync(SavedCharacter savedCharacter)
        {
            return await Task.Run(() => Add(savedCharacter));
        }

    }
}
