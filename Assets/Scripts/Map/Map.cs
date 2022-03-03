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
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Neighbours neighbours = new Neighbours
                {
                    bottom = GetCell(x, y - 1),
                    top = GetCell(x, y + 1),
                    left = GetCell(x - 1, y),
                    right = GetCell(x + 1, y),
                    bottomLeft = GetCell(x - 1, y - 1),
                    bottomRight = GetCell(x + 1, y - 1),
                    topLeft = GetCell(x - 1, y - 1),
                    topRight = GetCell(x + 1, y + 1)
                };

                GetCell(x, y).Tick(neighbours, this);
            }
        }
    }

    public void SwapCells(Vector2Int first, Vector2Int second)
    {
        Cell firstCell = GetCell(first.x, first.y);
        Cell secondCell = GetCell(second.x, second.y);

        SetCell(first.x, first.y, secondCell);
        SetCell(second.x, second.y, firstCell);
    }

    public void SwapCells(Cell first, Cell second)
    {
        SwapCells(first.position, second.position);
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
        cell.position = new Vector2Int(x, y);
    }

    public void SetElement(int x, int y, Element element)
    {
        Vector2Int position = new Vector2Int(x, y);
        Cell cell = new Cell(element, position);
        SetCell(x, y, cell);
    }
}
