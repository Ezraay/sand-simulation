using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Brushes
{
    public static bool[,] SquareBrush(int diameter)
    {
        bool[,] brush = new bool[diameter, diameter];

        for (int y = 0; y < diameter; y++)
        {
            for (int x = 0; x < diameter; x++)
            {
                brush[x, y] = true;
            }
        }

        return brush;
    }
}
