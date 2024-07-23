using Newtonsoft.Json;

namespace Project.API.Utils
{
    public class JsonDefaultSettings
    {
        public static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.All
        };
    }
}
