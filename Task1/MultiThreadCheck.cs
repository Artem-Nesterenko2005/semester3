using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    /// <summary>
    /// Static class for multi-thread md5 check-sum.
    /// </summary>
    static public class MultiThreadCheck
    {
        /// <summary>
        /// Calculates the checksum of files and directories according to a given rule with multi-thread mode.
        /// </summary>
        /// <param name="path">Path to file or directory.</param>
        /// <returns>Check-sum for path.</returns>
        /// <exception cref="InvalidDataException">Exception thrown when a path does not exist.</exception>
        static public async Task<byte[]> ComputeToFilePath(string path)
        {
            if (File.Exists(path))
            {
                var result = await File.ReadAllBytesAsync(path);
                var fileSum = System.Security.Cryptography.MD5.HashData(result);
                return fileSum;
            }

            if (Directory.Exists(path))
            {
                return await ComputeDirectories(path);
            }

            throw new InvalidDataException("Incorrect path");
        }

        private static async Task<byte[]> ComputeDirectories(string directory)
        {
            var listSums = new List<byte[]>();
            var directories = Directory.GetDirectories(directory);
            var files = Directory.GetFiles(directory);
            Array.Sort(directories);
            Array.Sort(files);
            for (var i = 0; i < files.Length; ++i)
            {
                var result = await ComputeToFilePath(files[i]);
                listSums.Add(result);
            }

            for (var i = 0; i < directories.Length; ++i)
            {
                var result = await ComputeDirectories(directories[i]);
                listSums.Add(result);
            }

            return Encoding.ASCII.GetBytes(directory);
        }
    }
}
