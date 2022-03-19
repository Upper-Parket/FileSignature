using System;
using System.Text;
using System.Threading;

namespace Signature
{
    public static class ConsoleWriter
    {
        public static void WriteToConsole(int partIndex, byte[] hash)
        {
            var sb = new StringBuilder();

            if (Program.WriteThreadInOutput)
                sb.Append(Thread.CurrentThread.Name).Append(" ");
            sb.Append("Part ").Append(partIndex).Append(" ").Append(hash.ConvertToString());

            Console.WriteLine(sb.ToString());
        }
    }
}