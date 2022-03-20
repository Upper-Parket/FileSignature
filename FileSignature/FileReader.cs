using System;
using System.IO;

namespace Signature
{
    public class FileReader
    {
        private readonly FileStream _fileStream;
        private int _readPart;
        private readonly object _lockObject = new object();
        
        public FileReader(string filePath)
        {
            _fileStream = File.OpenRead(filePath);
        }

        /// <summary>
        /// Reads next block from the file stream
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns>Number of the read block</returns>
        public int ReadBlock(byte[] buffer)
        {
            lock (_lockObject)
            {
                if (IsFileReadComplete())
                    return -1;
                
                Array.Clear(buffer, 0, buffer.Length);
                _fileStream.Read(buffer, 0, buffer.Length);
                return _readPart++;
            }
        }

        private bool IsFileReadComplete()
        {
            return _fileStream.Position == _fileStream.Length;
        }
    }
}