namespace CSharp.Test.TestConsoleApp
{
    using CSharp.Test.SerializableList;
    using CSharp.Test.TestConsoleApp.Helpers;
    using System;
    using System.Collections.Generic;
    using System.IO;

    partial class Program
    {
        private static void RunListRandSerializationTests()
        {
            Console.Write("ListRand tests...");

            var input = CreateListRand();
            string path = Path.Combine(Directory.GetCurrentDirectory(),"test.txt");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (FileStream fs = File.Create(path))
            {
                input.Serialize(fs);
            }

            var result = new ListRand();
            using (FileStream fs = File.OpenRead(path))
            {
                result.Deserialize(fs);
            }

            if (!ObjectFieldComparer.Compare(input, result, typeof(ListRand)))
            {
                throw new InvalidOperationException("Ожидаются одинаковые значения");
            }

            Console.WriteLine(" Ok");


            ListRand CreateListRand()
            {
                var nodeList = new List<ListNode>();
                for (var i = 0; i <= new Random().Next(100); i++)
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
