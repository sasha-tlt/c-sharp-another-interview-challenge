namespace CSharp.Test.TestConsoleApp.Helpers
{
    using System;
    using CSharp.Test.SerializableList.Serializer.Internal.IO;

    internal sealed class SerializerStringReader : ISerializerReader
    {
        private int _offset = 0;
        private readonly string _string;

        public SerializerStringReader(string value)
        {
            _string = value;
        }

        public bool CanRead()
        {
            return _offset < _string.Length;
        }

        public char Peek()
        {
            if (!CanRead())
            {
                throw new InvalidOperationException("Достигнут конец строки");
            }

            return _string[_offset];
        }

        public char Read()
        {
            if (!CanRead())
            {
                throw new InvalidOperationException("Достигнут конец строки");
            }

            return _string[_offset++];
        }
    }
}
