namespace CSharp.Test.TestConsoleApp.Helpers
{
    using CSharp.Test.SerializableList.Serializer.Internal.IO;
    using System;

    internal sealed class SerializerActionWriter : ISerializerWriter
    {
        private readonly Action<string> _writeAction;

        public SerializerActionWriter(Action<string> writeAction)
        {
            _writeAction = writeAction;
        }

        public void Write(string value)
        {
            _writeAction(value);
        }
    }
}
