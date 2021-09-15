using Newtonsoft.Json;

namespace Lend.API.Extenstions
{
    public static class GenericExtensions
    {
        public static T DeepCopy<T>(this T self)
        {
            var serialized = JsonConvert.SerializeObject(self);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
    }
}