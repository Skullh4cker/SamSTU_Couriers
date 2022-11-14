using System;

namespace SamSTU_Curiers
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
