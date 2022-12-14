using Newtonsoft.Json;
using XFrame.Modules.Serialize;

namespace UnityXFrame.Core.Serialize
{
    public class JsonSerializeHelper : IJsonSerializeHelper
    {
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
