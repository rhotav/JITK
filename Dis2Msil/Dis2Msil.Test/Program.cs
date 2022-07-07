using System;
using System.Reflection;

namespace Dis2Msil.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string testIl = "02-28-1C-00-00-0A-00-00-02-03-7D-11-00-00-04-2A";
            byte[] testIlByte = { 0x02, 0x28, 0x1C, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x02, 0x03, 0x7D, 0x11, 0x00, 0x00, 0x04, 0x2A };
            //string fileName = args[0];
            string fileName = @"C:\Users\utku\Desktop\WorkSpace\JITK\asd.exe";
            
            MethodBodyReader mr = new MethodBodyReader(Assembly.LoadFrom(fileName).EntryPoint.Module, testIl);
            Console.Write(mr.GetBodyCode());

            Console.WriteLine("========================================");
            
            MethodBodyReader mrByte = new MethodBodyReader(Assembly.LoadFrom(fileName).EntryPoint.Module, testIlByte);
            Console.Write(mrByte.GetBodyCode());

            Console.ReadKey();
        }
    }
}
