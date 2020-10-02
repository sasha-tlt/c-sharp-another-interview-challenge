namespace CSharp.Test.SerializableList.Serializer.Internal
{
    using CSharp.Test.SerializableList.Serializer.Internal.Extensions;
    using CSharp.Test.SerializableList.Serializer.Internal.IO;
    using System;
    using System.Collections.Generic;

    internal sealed class SerializationWriter
    {
        private readonly Serializer _serializer;
        private readonly ISerializerWriter _writer;
        private readonly List<Object> _serializedObjectReferences = new List<object>();

        public SerializationWriter(Serializer serializer, ISerializerWriter writer)
        {
            _serializer = serializer;
            _writer = writer;
        }

        public void Write(object @object, Type objectType)
        {
            if (@object == null)
            {
                WriteNull();
                return;
            }

            if (objectType == typeof(string))
            {
                _writer.Write($"\"{@object.ToString()}\"");
            }
            else if (objectType.IsIntegerType())
            {
                _writer.Write($"{@object.ToString()}");
            }
            else if (objectType.IsFloatType())
            {
                throw new NotSupportedException();
            }
            else if (!objectType.IsClass)
            {
                throw new NotSupportedException();
            }
            else
            {
                if (_serializedObjectReferences.Contains(@object))
                {
                    WriteReference(@object);
                }
                else
                {
                    _serializedObjectReferences.Add(@object);

                    WriteStart();
                    WriteReferenceIdField(@object);
                    _serializer.Serialize(@object, objectType);
                    WriteEnd();
                }
            }
        }

        public void Write(string fieldName, object fieldValue)
        {
            WriteFieldDelimiter();
            WriteFieldHeader(fieldName);
            Write(fieldValue, fieldValue?.GetType());
        }

        private void WriteStart()
        {
            _writer.Write("{");
        }

        private void WriteNull()
        {
            _writer.Write("null");
        }

        private void WriteEnd()
        {
            _writer.Write("}");
        }

        private void WriteFieldHeader(string fieldName)
        {
            _writer.Write($"\"{fieldName}\":");
        }

        private void WriteFieldDelimiter()
        {
            _writer.Write(",");
        }

        private void WriteReferenceIdField(object @object)
        {
            _writer.Write($"\"#reference\":{@object.GetHashCode()}");
        }

        private void WriteReference(object @object)
        {
            _writer.Write($"#{@object.GetHashCode()}");
        }
    }
}
