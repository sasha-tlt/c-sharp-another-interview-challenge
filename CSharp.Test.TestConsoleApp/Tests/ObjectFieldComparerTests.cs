namespace CSharp.Test.TestConsoleApp
{
    using CSharp.Test.TestConsoleApp.Helpers;
    using System;

    partial class Program
    {
        class Test
        {
            public string Test1;
            public int Test2;
            public string Test3;
            public Test Test4;
        }

        static void RunObjectFieldComparerTests()
        {
            Console.Write("Object comparer tests...");

            var test1 = new Test()
            {
                Test1 = "test1",
                Test2 = 2,
                Test3 = "Test3",
                Test4 = new Test()
                {
                    Test1 = "test41",
                    Test2 = 20,
                    Test3 = "",
                    Test4 = null
                }
            };

            var test2 = new Test()
            {
                Test1 = "test1",
                Test2 = 2,
                Test3 = "Test3",
                Test4 = new Test()
                {
                    Test1 = "test41",
                    Test2 = 20,
                    Test3 = "",
                    Test4 = null
                }
            };

            if (!ObjectFieldComparer.Compare(test1, test2, typeof(Test)))
            {
                throw new InvalidOperationException("Ожидаются одинаковые данные");
            }

            test2.Test4 = test1;

            if (ObjectFieldComparer.Compare(test1, test2, typeof(Test)))
            {
                throw new InvalidOperationException("Ожидаются разные данные");
            }

            Console.WriteLine(" Ok");
        }
    }
}
