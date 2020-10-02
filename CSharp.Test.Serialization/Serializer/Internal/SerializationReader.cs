namespace CSharp.Test.SerializableList.Serializer.Internal
{
    using CSharp.Test.SerializableList.Serializer.Internal.Extensions;
    using CSharp.Test.SerializableList.Serializer.Internal.IO;
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal sealed class SerializationReader
    {
        private readonly Deserializer _deserializer;
        private readonly ISerializerReader _reader;
        private readonly Dictionary<string, object> _objectReferences = new Dictionary<string, object>();

        public SerializationReader(Deserializer deserializer, ISerializerReader reader)
        {
            _deserializer = deserializer;
            _reader = reader;
        }

        public object Read(Type objectType)
        {
            if (objectType == typeof(string))
            {
                return ReadString();
            }
            else if (objectType.IsIntegerType())
            {
                return ReadInteger(objectType);
            }
            else if (objectType.IsFloatType())
            {
                throw new NotSupportedException();
            }
            else if (!objectType.IsClass)
            {
                throw new NotSupportedException();
            }

            return ReadObject(objectType);
        }

        private object ReadObject(Type objectType)
        {
            var @object = Activator.CreateInstance(objectType);
            var buffer = new StringBuilder();
            var firstSymbol = true;
            while (_reader.CanRead())
            {
                var element = _reader.Peek();
                switch (element)
                {
                    case '#':
                        buffer.Append(_reader.Read());
                        if (firstSymbol)
                        {
                            return ReadReferenceValue();
                        }
                        
                        break;

                    case ':':
                        _reader.Read();
                        var fieldName = buffer.ToString().Trim('"', ' ');
                        if (string.Compare(fieldName, "#reference", StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            ReadReferenceId(@object);
                        }
                        else
                        {
                            _deserializer.Deserialize(@object, objectType, fieldName);
                        }

                        if (_reader.CanRead() && _reader.Peek() == ',')
                        {
                            _reader.Read();
                        }
                        
                        buffer.Clear();
                        break;
                    case '{':
                        _reader.Read();
                        break;
                    case '}':
                    case ',':
                        _reader.Read();
                        if (string.Compare(buffer.ToString(), "null", StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return null;
                        }

                        return @object;
                        
                    default:
                        firstSymbol = firstSymbol && element == ' ';
                        buffer.Append(_reader.Read());
                        break;
                }
            }

            if (string.Compare(buffer.ToString(), "null", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return null;
            }

            throw new InvalidOperationException("Неожиданный конец сериализованной последовательности");
        }

        private void ReadReferenceId(object @object)
        {
            var buffer = new StringBuilder();
            while (_reader.CanRead())
            {
                var element = _reader.Peek();
                switch (element)
                {
                    case '}':
                    case ',':
                        _objectReferences.Add(buffer.ToString().Trim('"', ' '), @object);
                        return;
                    default:
                        buffer.Append(_reader.Read());
                        break;
                }
            }

            throw new InvalidOperationException("Неожиданный конец сериализованной последовательности");
        }

        private object ReadReferenceValue()
        {
            var buffer = new StringBuilder();
            while (_reader.CanRead())
            {
                var element = _reader.Peek();
                switch (element)
                {
                    case '}':
                    case ',':
                        if (element == ',')
                        {
                            _reader.Read();
                        }

                        return _objectReferences[buffer.ToString().Trim('"', ' ')];
                    default:
                        buffer.Append(_reader.Read());
                        break;
                }
            }

            throw new InvalidOperationException("Неожиданный конец сериализованной последовательности");
        }

        private string ReadString()
        {
            var buffer = new StringBuilder();
            int bracketCnt = 0;
            var bracketActive = false;
            var stringEnded = false;
            while (_reader.CanRead() && !stringEnded)
            {
                var element = _reader.Peek();
                switch (element)
                {
                    case '"':
                        bracketCnt++;
                        bracketActive = true;
                        buffer.Append(_reader.Read());
                        break;

                    case ',':
                    case '}':
                        if (bracketActive && bracketCnt >= 2)
                        {
                            stringEnded = true;
                            break;
                        }

                        buffer.Append(_reader.Read());
                        break;

                    default:
                        bracketActive = bracketActive && element == ' ';
                        buffer.Append(_reader.Read());
                        break;
                }
            }

            var value = buffer.ToString().Trim('"', ' ');
            return string.Compare(value, "null", StringComparison.OrdinalIgnoreCase) == 0 ? null : value;
        }

        private object ReadInteger(Type objectType)
        {
            var buffer = new StringBuilder();
            while (_reader.CanRead())
            {
                var element = _reader.Peek();
                switch (element)
                {
                    case '}':
                    case ',':
                        return Convert.ToInt32(buffer.ToString());
                    default:
                        buffer.Append(_reader.Read());
                        break;
                }
            }

            throw new InvalidOperationException("Неожиданный конец сериализованной последовательности");
        }
    }
}
