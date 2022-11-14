using System;

namespace SamSTU_Couriers
{
    class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char CoordSymbol { get; set; }
        public Position(int x, int y, char symbol)
        {
            X = x;
            Y = y;
            CoordSymbol = symbol;
        }
        // Функция для рассчёта дистанции между двумя точками
        public static double GetDistance(Position pos1, Position pos2)
        {
            return Math.Sqrt(Math.Pow(pos2.X - pos1.X, 2) + Math.Pow(pos2.Y - pos1.Y, 2));
        }
    }
}
