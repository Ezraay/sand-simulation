using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Map
{
    public readonly int width;
    public readonly int height;

    private readonly Cell[,] cells;

    public Map(int width, int height)
    {
        this.width = width;
        this.height = height;

        this.cells = new Cell[width, height];
    }

    public void Tick()
    {

    }

    public Cell GetCell(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height)
        {
            return null;
        }

        return cells[x, y];
    }

    public void SetCell(int x, int y, Cell cell)
    {
        if (x < 0 || y < 0 || x >= width || y >= height)
        {
            return;
        }

        cells[x, y] = cell;
    }
}
