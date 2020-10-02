namespace CSharp.Test.SerializableList.Serializer.Internal.IO
{
    using System.IO;
    using System.Text;

    internal sealed class SerializerStreamWriter: ISerializerWriter
    {
        private readonly Stream _writeStream;

        public SerializerStreamWriter(Stream stream)
        {
            _writeStream = stream;
        }

        public void Write(string value)
        {
            var data = new UTF8Encoding(true).GetBytes(value);
            _writeStream.Write(data, 0, data.Length);
        }
    }
}
