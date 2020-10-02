[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("CSharp.Test.TestConsoleApp")]
namespace CSharp.Test.SerializableList
{
    using CSharp.Test.SerializableList.Serializer;
    using System.IO;

    public class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(FileStream s)
        {
            StreamSerializer.Serialize(s, this);
        }

        public void Deserialize(FileStream s)
        {
            StreamSerializer.Deserialize(s, this);
        }
    }
}
