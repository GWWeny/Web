using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 创建Random对象
            Random random = new Random();

            // 生成一个1到100之间的随机整数
            int randomNumber = random.Next(1, 101);
            Console.WriteLine($"随机整数：{randomNumber}");

            // 生成一个0到1之间的随机浮点数
            double randomDouble = random.NextDouble();
            Console.WriteLine($"随机浮点数：{randomDouble:F2}");

            // 生成一个0到1之间的随机浮点数，并扩展到0到100的范围
            double randomDoubleScaled = random.NextDouble() * 100;
            Console.WriteLine($"随机浮点数（0-100）：{randomDoubleScaled:F2}");

            // 生成一个随机布尔值
            bool randomBool = random.Next(2) == 0;
            Console.WriteLine($"随机布尔值：{randomBool}");

            // 等待用户输入，以便查看结果
            Console.WriteLine("\n按任意键退出程序...");
            Console.ReadKey();
        }
    }
}
