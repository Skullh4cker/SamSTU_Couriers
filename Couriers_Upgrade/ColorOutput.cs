using System;

namespace Courier_Upgrade
{
    class ColorOutput
    {
        // Удобный вывод цветных строк
        public static void Color_Writeline(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
