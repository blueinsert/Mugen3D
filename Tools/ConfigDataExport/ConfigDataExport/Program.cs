using System;
using System.IO;

namespace bluebean.CSVParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            foreach(var arg in args)
            {
                Console.WriteLine(arg);
            }
            string inputPath = "./";//默认当前路径
            bool inputIsFolder = false;
            string outPath = "./";
            string format = "json";//数据序列化格式
            for (int i = 0; i < args.Length; i++) {
                switch (args[i])
                {
                    case "-i":
                    case "-I":
                        inputPath = args[i + 1];
                        break;
                    case "-o":
                    case "-O":
                        outPath = args[i + 1];
                        break;
                    case "-f":
                    case "-F":
                        format = args[i + 1];
                        break;
                }
            }
            inputIsFolder = Directory.Exists(inputPath);
            if (!inputIsFolder)
            {
                if (!inputPath.EndsWith(".csv")) {
                    Console.WriteLine("error:input file name not end with csv");
                    return;
                }
            }
            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }
            ConfigDataManager.CreateInstance();
            if (!inputIsFolder)
            {
                //ConfigDataManager.Instance.ProcessSingleFile(inputPath, outPath, format);
            }
            ConfigDataManager.Instance.ProcessSingleFile("Character.csv", "./Output", "json");
        }
    }
}
