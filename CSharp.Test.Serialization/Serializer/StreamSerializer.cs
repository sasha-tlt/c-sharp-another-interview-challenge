namespace CSharp.Test.SerializableList.Serializer
{
    using CSharp.Test.SerializableList.Serializer.Internal;
    using CSharp.Test.SerializableList.Serializer.Internal.IO;
    using System;
    using System.IO;

    internal static class StreamSerializer
    {
        public static void Serialize<T>(Stream stream, T serializableObject)
        {
            if (stream == null || !stream.CanWrite)
            {
                throw new ArgumentException("stream");
            }

            new Serializer(new SerializerStreamWriter(stream)).Serialize<T>(serializableObject);
        }

        public static T Deserialize<T>(Stream stream)
        {
            if (stream == null || !stream.CanRead)
            {
                throw new ArgumentException("stream");
            }

            return new Deserializer(new SerializerStreamReader(stream)).Deserialize<T>();
        }

        public static void Deserialize<T>(Stream stream, T deserializableObject)
        {
            if (stream == null || !stream.CanRead)
            {
                throw new ArgumentException("stream");
            }

            if (deserializableObject == null)
            {
                throw new ArgumentNullException("deserializableObject");
            }

            new Deserializer(new SerializerStreamReader(stream)).Deserialize<T>(deserializableObject);
        }
    }
}
