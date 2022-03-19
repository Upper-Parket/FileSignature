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
                if (!CanReadBlock(buffer.Length))
                    return -1;

                _fileStream.Read(buffer, 0, buffer.Length);
                return _readPart++;
            }
        }

        private bool CanReadBlock(int blockLength)
        {
            return _fileStream.Position + blockLength < _fileStream.Length;
        }
    }
}