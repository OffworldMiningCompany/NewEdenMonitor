using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace NewEdenMonitor.Model
{
    internal class Image
    {
        public string Type { get; set; }
        public long Id { get; set; }
        public int Size { get; set; }
        public byte[] ImageBinary { get; set; }
    }

    internal class ImageHandler
    {
        private readonly EveContext _db;

        internal ImageHandler(EveContext db)
        {
            _db = db;
        }

        internal Image Get(string type, long id, int size)
        {
            Image image = null;

            using (var command = _db.Connection.CreateCommand())
            {
                command.CommandText =
                    "SELECT type, id, size, image FROM images WHERE type = @type AND id = @id AND size = @size;";

                var param = command.Parameters.Add("type", DbType.String);
                param.Value = type;

                param = command.Parameters.Add("id", DbType.Int64);
                param.Value = id;

                param = command.Parameters.Add("size", DbType.Int32);
                param.Value = size;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        image = ReadObject(reader);
                    }

                    reader.Close();
                }
            }

            return image;
        }

        internal async Task<Image> GetAsync(string type, long id, int size)
        {
            return await Task.Run(() => Get(type, id, size));
        }

        internal bool Set(Image image)
        {
            using (var command = _db.Connection.CreateCommand())
            {
                command.CommandText = "INSERT OR REPLACE INTO images VALUES (@type, @id, @size, @image);";

                var param = command.Parameters.Add("type", DbType.String);
                param.Value = image.Type;

                param = command.Parameters.Add("id", DbType.Int64);
                param.Value = image.Id;

                param = command.Parameters.Add("size", DbType.Int32);
                param.Value = image.Size;

                param = command.Parameters.Add("image", DbType.Binary);
                param.Value = image.ImageBinary;

                int res = command.ExecuteNonQuery();

                if (res == 1)
                {
                    return true;
                }
            }

            return false;
        }

        internal async Task<bool> SetAsync(Image image)
        {
            return await Task.Run(() => Set(image));
        }

        private Image ReadObject(SQLiteDataReader reader)
        {
            var image = new Image();

            image.Type = reader["type"].ToString();
            image.Id = reader["id"].ToInt64();
            image.Size = reader["size"].ToInt32();
            image.ImageBinary = reader["image"].ToByteArray();

            return image;
        }
    }
}
