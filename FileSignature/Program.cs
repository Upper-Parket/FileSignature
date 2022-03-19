using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Signature
{
    internal static class Program
    {
        public const bool WriteThreadInOutput = false;
        
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            var (filePath, blockSize) = ParseArguments(args);

            var reader = new FileReader(filePath);
            var processors = Environment.ProcessorCount;
            var awaitable = new WaitHandle[processors];
            var sw = Stopwatch.StartNew();
            
            for (var i = 0; i < processors; i++)
            {
                var autoResetEvent = new AutoResetEvent(false);
                var thread = new Thread(() => FileHasher.Work(reader, blockSize, autoResetEvent))
                {
                    Name = $"Thread {i}",
                    IsBackground = true
                };
                thread.Start();
                awaitable[i] = autoResetEvent;
            }

            WaitHandle.WaitAll(awaitable);
            sw.Stop();
            Console.WriteLine($"Execution took: {sw.Elapsed.TotalSeconds} seconds");
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject);
        }

        private static (string filePath, int blockSize) ParseArguments(string[] args)
        {
            if (args.Length < 2) throw new ArgumentException("Two arguments required: file path and block size");

            var filePath = args[0];
            if (!File.Exists(filePath)) throw new FileNotFoundException("File not found", filePath);

            var blockSize = int.Parse(args[1]);
            if (blockSize <= 0) throw new ArgumentException("Block size must be higher than 0");

            return (filePath, blockSize);
        }
    }
}