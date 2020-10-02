namespace CSharp.Test.SerializableList.Serializer.Internal.IO
{
    using System;
    using System.IO;

    internal sealed class SerializerStreamReader: ISerializerReader
    {
        private readonly StreamReader _streamReader;

        public SerializerStreamReader(Stream stream)
        {
            _streamReader = new StreamReader(stream);
        }

        public bool CanRead()
        {
            return _streamReader.Peek() > -1;
        }

        public char Peek()
        {
            if (!CanRead())
            {
                throw new InvalidOperationException("Достигнут конец потока");
            }

            return (char)_streamReader.Peek();
        }

        public char Read()
        {
            if (!CanRead())
            {
                throw new InvalidOperationException("Достигнут конец потока");
            }

            return (char)_streamReader.Read();
        }
    }
}
