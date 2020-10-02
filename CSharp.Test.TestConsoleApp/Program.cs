namespace CSharp.Test.TestConsoleApp
{
    using System;

    partial class Program
    {
        static void Main(string[] args)
        {
            RunObjectFieldComparerTests(); // тесты сравнения объектов
            RunSerializeTests(); // тесты сериализатора
            RunListRandSerializationTests(); // тесты листа

            Console.ReadKey();
        }
    }
}
