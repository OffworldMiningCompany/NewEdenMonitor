using System.Linq;
using System.Reflection;

namespace NewEdenMonitor.Utils
{
    public static class Resource
    {
        public static byte[] GetEmbeddedResource(string name)
        {
            byte[] buffer = null;

            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] resources = assembly.GetManifestResourceNames();
            var resource = resources.FirstOrDefault(r => r.EndsWith(name));

            if (resource == null)
            {
                throw new InvalidResourceException(resource, "The specified resource was not found.");
            }

            using (var memoryStream = assembly.GetManifestResourceStream(resource))
            {
                if (memoryStream == null)
                {
                    throw new InvalidResourceException(resource, "The specified resource could not be loaded.");
                }

                buffer = new byte[memoryStream.Length];
                memoryStream.Read(buffer, 0, buffer.Length);
            }


            return buffer;
        }
    }
}
