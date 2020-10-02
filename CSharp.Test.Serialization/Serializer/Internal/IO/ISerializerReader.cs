namespace CSharp.Test.SerializableList.Serializer.Internal.IO
{
    internal interface ISerializerReader
    {
        bool CanRead();
        char Peek();
        char Read();
    }
}
