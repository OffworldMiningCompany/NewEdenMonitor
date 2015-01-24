using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eZet.EveLib.EveXmlModule;

namespace NewEdenMonitor.Model
{
    class ImageCache
    {
        public static async Task<byte[]> GetAllianceLogoDataAsync(long allianceId, eZet.EveLib.EveXmlModule.Image.AllianceLogoSize size)
        {
            const string type = "Alliance";

            using (var db = new EveContext())
            {
                var image = await db.ImageHandler.GetAsync(type, allianceId, (int)size);

                if (image == null)
                {
                    var buffer = await EveXml.Image.GetAllianceLogoDataAsync(allianceId, size);

                    image = new Image();
                    image.Type = type;
                    image.Id = allianceId;
                    image.Size = (int)size;
                    image.ImageBinary = buffer;

                    await db.ImageHandler.SetAsync(image);
                }

                return image.ImageBinary;
            }
        }

        public static async Task<byte[]> GetCharacterPortraitDataAsync(long characterId, eZet.EveLib.EveXmlModule.Image.CharacterPortraitSize size)
        {
            const string type = "Character";

            using (var db = new EveContext())
            {
                var image = await db.ImageHandler.GetAsync(type, characterId, (int)size);

                if (image == null)
                {
                    var buffer = await EveXml.Image.GetCharacterPortraitDataAsync(characterId, size);

                    image = new Image();
                    image.Type = type;
                    image.Id = characterId;
                    image.Size = (int) size;
                    image.ImageBinary = buffer;

                    await db.ImageHandler.SetAsync(image);
                }

                return image.ImageBinary;
            }
        }

        public static async Task<byte[]> GetCorporationLogoDataAsync(long corporationId, eZet.EveLib.EveXmlModule.Image.CorporationLogoSize size)
        {
            const string type = "Corporation";

            using (var db = new EveContext())
            {
                var image = await db.ImageHandler.GetAsync(type, corporationId, (int)size);

                if (image == null)
                {
                    var buffer = await EveXml.Image.GetCorporationLogoDataAsync(corporationId, size);

                    image = new Image();
                    image.Type = type;
                    image.Id = corporationId;
                    image.Size = (int)size;
                    image.ImageBinary = buffer;

                    await db.ImageHandler.SetAsync(image);
                }

                return image.ImageBinary;
            }
        }

        public static async Task<byte[]> GetRenderDataAsync(long typeId, eZet.EveLib.EveXmlModule.Image.RenderSize size)
        {
            const string type = "Render";

            using (var db = new EveContext())
            {
                var image = await db.ImageHandler.GetAsync(type, typeId, (int)size);

                if (image == null)
                {
                    var buffer = await EveXml.Image.GetRenderDataAsync(typeId, size);

                    image = new Image();
                    image.Type = type;
                    image.Id = typeId;
                    image.Size = (int)size;
                    image.ImageBinary = buffer;

                    await db.ImageHandler.SetAsync(image);
                }

                return image.ImageBinary;
            }
        }

        public static async Task<byte[]> GetTypeIconDataAsync(long typeId, eZet.EveLib.EveXmlModule.Image.AllianceLogoSize size)
        {
            const string type = "InventoryType";

            using (var db = new EveContext())
            {
                var image = await db.ImageHandler.GetAsync(type, typeId, (int)size);

                if (image == null)
                {
                    var buffer = await EveXml.Image.GetTypeIconDataAsync(typeId, size);

                    image = new Image();
                    image.Type = type;
                    image.Id = typeId;
                    image.Size = (int)size;
                    image.ImageBinary = buffer;

                    await db.ImageHandler.SetAsync(image);
                }

                return image.ImageBinary;
            }
        }
    }
}
