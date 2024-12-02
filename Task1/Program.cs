using System.Diagnostics;
using System.Security.Cryptography;
using Task1;

namespace Task1
{
    /// <summary>
    /// Program class for running.
    /// </summary>
    class Program
    {
        static private void Main()
        {
            Stopwatch stopwatch = new ();

            stopwatch.Start();
            var checkSum1 = MultiThreadCheck.ComputeToFilePath("C:\\Users\\Huawei\\Desktop\\3sem\\Task1\\MainDirectory");
            stopwatch.Stop();
            var resultMultiThread = stopwatch.Elapsed;

            stopwatch.Restart();
            var checkSum2 = SingleThreadCheck.ComputeToFilePath("C:\\Users\\Huawei\\Desktop\\3sem\\Task1\\MainDirectory");
            stopwatch.Stop();
            var resultSingleThread = stopwatch.Elapsed;

            Console.WriteLine($"SingleThread time - {resultSingleThread}");
            Console.WriteLine($"MultiThread time - {resultMultiThread}");
        }
    }
}
