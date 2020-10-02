namespace CSharp.Test.TestConsoleApp.Helpers
{
    using System;
    using System.Text;

    internal static class StringGenerator
    {
        private static Random _symbolRandom = new Random((int)DateTime.Now.Ticks);
        private static Random _countRandom = new Random();
        public static string Generate()
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < _countRandom.Next(20); i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _symbolRandom.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}
