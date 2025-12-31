using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitwiseByClaude
{
    public static class Extensions
    {
        public static void Copy2DArray(this int[,] source, int[,] destination)
        {
            int rows = source.GetLength(0);
            int cols = source.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    destination[i, j] = source[i, j];
                }
            }
        }
    }
}
