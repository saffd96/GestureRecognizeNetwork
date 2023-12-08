using System.IO;
using NeuralNetwork.Interfaces;
using Newtonsoft.Json;

namespace NeuralNetwork
{
    public class JSONSerializer<T> : ISerializer<T>, IDeserializer<T>
    {
        public string Serialize(T gesture)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(gesture, serializerSettings);
        }

        public T Deserialize(string path)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }

    }
}