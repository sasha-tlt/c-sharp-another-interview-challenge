namespace CSharp.Test.TestConsoleApp
{
    using CSharp.Test.SerializableList;
    using CSharp.Test.SerializableList.Serializer.Internal;
    using CSharp.Test.TestConsoleApp.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Text;

    partial class Program
    {
        static void RunSerializeTests()
        {
            Console.Write("Serialize tests...");

            SerializeDeserializeEqualsTest(CreateListRand1());
            SerializeDeserializeEqualsTest(CreateListRand2());

            Console.WriteLine(" Ok");

            void SerializeDeserializeEqualsTest<T>(T value)
            {
                var buffer = new StringBuilder();
                new Serializer(new SerializerActionWriter(s => buffer.Append(s))).Serialize(value);

                var dValue = new Deserializer(new SerializerStringReader(buffer.ToString())).Deserialize<T>();

                if (!ObjectFieldComparer.Compare(value, dValue, typeof(T)))
                {
                    throw new InvalidOperationException("Ожидаются одинаковые значения");
                }
            }

            ListRand CreateListRand1()
            {
                return null;
            }

            ListRand CreateListRand2()
            {
                var nodeList = new List<ListNode>();
                for (var i = 0; i <= new Random().Next(2000); i++)
                {
                    nodeList.Add(CreateListNode());
                }

                var random = new Random();
                for (var i = 0; i < nodeList.Count; i++)
                {
                    nodeList[i].Next = i < nodeList.Count - 1 ? nodeList[i + 1] : null;
                    nodeList[i].Prev = i > 0 ? nodeList[i - 1] : null;
                    nodeList[i].Rand = nodeList[random.Next(nodeList.Count - 1)];
                }

                return new ListRand()
                {
                    Head = nodeList[0],
                    Tail = nodeList[nodeList.Count - 1],
                    Count = new Random().Next(100)
                };
            }

            ListNode CreateListNode()
            {
                return new ListNode
                {
                    Data = StringGenerator.Generate()
                };
            }
        }

    }
}
