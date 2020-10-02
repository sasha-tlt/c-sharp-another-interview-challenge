namespace CSharp.Test.SerializableList.Serializer.Internal
{
    using CSharp.Test.SerializableList.Serializer.Internal.IO;
    using System;
    using System.Linq;

    internal sealed class Deserializer
    {
        private readonly SerializationReader _reader;

        public Deserializer(ISerializerReader reader)
        {
            _reader = new SerializationReader(this, reader);
        }

        public T Deserialize<T>()
        {
            return (T)_reader.Read(typeof(T));
        }

        public void Deserialize<T>(object deserializableObject)
        {
            var deserializedObject = Deserialize<T>();

            foreach(var field in typeof(T).GetFields().Where(f => f.IsPublic))
            {
                field.SetValue(deserializableObject, field.GetValue(deserializedObject));
            }
        }

        internal void Deserialize(object deserializableObject, Type objectType, string fieldName)
        {
            var field = objectType.GetFields().FirstOrDefault(f => f.IsPublic && f.Name == fieldName);
            if (field == null)
            {
                throw new InvalidOperationException("Ошибка маппинга полей: Отсутствует поле в типе");
            }

            field.SetValue(deserializableObject, _reader.Read(field.FieldType));
        }
    }
}
