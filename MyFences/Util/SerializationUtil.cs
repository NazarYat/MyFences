using Newtonsoft.Json;
using System.IO;

namespace MyFences.Util
{
    public static class SerializationUtil
    {
        public static T? DeserializeFormJson<T>(string json) => JsonConvert.DeserializeObject<T>(json);
        public static string SerializeToJson(object obj) => JsonConvert.SerializeObject(obj);

        public static async Task SaveToFileAsync(string path, object obj)
        {
            string json = SerializeToJson(obj);

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            }

            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }

            await File.WriteAllTextAsync(path, json);
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
