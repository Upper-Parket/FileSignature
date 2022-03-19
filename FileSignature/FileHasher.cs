using System;
using System.Threading;

namespace Signature
{
    public class FileHasher
    {
        private readonly byte[] _buffer;
        private readonly FileReader _fileReader;
        private readonly AutoResetEvent _autoResetEvent;

        private FileHasher(FileReader fileReader, int blockSize, AutoResetEvent autoResetEvent)
        {
            _buffer = new byte[blockSize];
            _fileReader = fileReader;
            _autoResetEvent = autoResetEvent;
        }

        private void DoWork()
        {
            while (true)
            {
                var number = _fileReader.ReadBlock(_buffer);
                if (number == -1)
                    break;
                var generatedHash = HashGenerator.Generate(_buffer);
                ConsoleWriter.WriteToConsole(number, generatedHash);
            }

            _autoResetEvent.Set();
        }

        public static void Work(FileReader reader, int blockSize, AutoResetEvent autoResetEvent)
        {
            var hasher = new FileHasher(reader, blockSize, autoResetEvent);
            hasher.DoWork();
        }
        
        
        public static void TryWork(FileReader reader, int blockSize, AutoResetEvent autoResetEvent)
        {
            FileHasher hasher;
            try
            {
                hasher = new FileHasher(reader, blockSize, autoResetEvent);
            }
            catch (OutOfMemoryException)
            {
                autoResetEvent.Set();
                return;
            }

            hasher.DoWork();
        }
    }
}