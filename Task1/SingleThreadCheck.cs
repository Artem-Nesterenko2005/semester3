using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    /// <summary>
    /// Static class for single-thread md5 check-sum.
    /// </summary>
    static public class SingleThreadCheck
    {
        /// <summary>
        /// Calculates the checksum of files and directories according to a given rule with single-thread mode.
        /// </summary>
        /// <param name="path">Path to file or directory.</param>
        /// <returns>Check-sum for path.</returns>
        /// <exception cref="InvalidDataException">Exception thrown when a path does not exist.</exception>
        static public byte[] ComputeToFilePath(string path)
        {
            if (File.Exists(path)) 
            {
                var result = File.ReadAllBytes(path);
                var fileHash = System.Security.Cryptography.MD5.HashData(result);
                return fileHash;
            }

            if (Directory.Exists(path)) 
            {
                return ComputeDirectories(path);
            }

            throw new InvalidDataException("Incorrect path");
        }

        private static byte[] ComputeDirectories(string directory)
        {
            var listSums = new List<byte[]>();
            var directories = Directory.GetDirectories(directory);
            var files = Directory.GetFiles(directory);
            Array.Sort(directories);
            Array.Sort(files);
            for (var i = 0; i < files.Length; ++i)
            {
                listSums.Add(ComputeToFilePath(files[i]));
            }

            for (var i = 0; i < directories.Length; ++i)
            {
                listSums.Add(ComputeDirectories(directories[i]));
            }
            return Encoding.ASCII.GetBytes(directory);
        }
    }
}
