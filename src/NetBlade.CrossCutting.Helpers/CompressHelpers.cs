using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.IO.Compression;

namespace NetBlade.CrossCutting.Helpers
{
    public static class CompressHelpers
    {
        public static T DeserializeJsonAndDecompress<T>(byte[] objectSerialize)
        {
            using MemoryStream memStream = new MemoryStream(objectSerialize);
            using GZipStream zipStream = new GZipStream(memStream, CompressionMode.Decompress, true);
            using StreamReader streamReader = new StreamReader(zipStream);
            using JsonReader jsonReader = new JsonTextReader(streamReader);
            JsonSerializer jsonSerializer = new JsonSerializer { ContractResolver = new CamelCasePropertyNamesContractResolver(), Formatting = Formatting.None };
            return jsonSerializer.Deserialize<T>(jsonReader);
        }

        public static byte[] SerializerJsonAndCompress(object objectToSerialize)
        {
            using MemoryStream memStream = new MemoryStream();
            using (GZipStream zipStream = new GZipStream(memStream, CompressionMode.Compress, true))
            using (StreamWriter streamWriter = new StreamWriter(zipStream))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(streamWriter))
            {
                JsonSerializer jsonSerializer = new JsonSerializer { ContractResolver = new CamelCasePropertyNamesContractResolver(), Formatting = Formatting.None };
                jsonSerializer.Serialize(jsonWriter, objectToSerialize);
            }

            return memStream.ToArray();
        }
    }
}
