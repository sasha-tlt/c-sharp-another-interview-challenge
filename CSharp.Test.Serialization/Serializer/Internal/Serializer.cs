namespace CSharp.Test.SerializableList.Serializer.Internal
{
    using CSharp.Test.SerializableList.Serializer.Internal.IO;
    using System;
    using System.Linq;

    internal sealed class Serializer
    {
        private readonly SerializationWriter _writer;

        public Serializer(ISerializerWriter writer)
        {
            _writer = new SerializationWriter(this, writer);
        }

        public void Serialize<T>(T serializableObject)
        {
            _writer.Write(serializableObject, typeof(T));
        }

        internal void Serialize(object serializableObject, Type objectType)
        {
            foreach (var field in objectType.GetFields().Where(f => f.IsPublic))
            {
                var value = field.GetValue(serializableObject);
                _writer.Write(field.Name, value);
            }
        }
    }
}
