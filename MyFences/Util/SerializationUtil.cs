using Newtonsoft.Json;
using System.IO;

namespace MyFences.Util
{
    public static class SerializationUtil
    {
        public static T? DeserializeFormJson<T>(string json) => JsonConvert.DeserializeObject<T>(json);
        public static string SerializeToJson(object obj) => JsonConvert.SerializeObject(obj);

        public static void SaveToFile(string path, object obj)
        {
            string json = SerializeToJson(obj);
            File.WriteAllText(path, json);
        }

        public static T? LoadFromFile<T>(string path)
        {
            if (!File.Exists(path))
                return default;

            string json = File.ReadAllText(path);
            return DeserializeFormJson<T>(json);
        }
    }
}
