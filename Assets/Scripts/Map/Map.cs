using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Map
{
    public readonly int width;
    public readonly int height;

    public readonly List<Cell> solidCells = new List<Cell>();
    private readonly Cell[,] cells;
    private List<CellMove> cellMoves = new List<CellMove>();

    public Map(int width, int height)
    {
        this.width = width;
        this.height = height;

        this.cells = new Cell[width, height];
    }

    public void Tick()
    {
        cellMoves.Clear();

        StructuralIntegrity.Tick(this);

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

                Cell cell = GetCell(x, y);
                cell.hasMoved = false;
                cell.Tick(neighbours, this);
            }
        }

        while (cellMoves.Count > 0)
        {
            int index = Random.Range(0, cellMoves.Count - 1);
            CellMove randomMove = cellMoves[index];
            cellMoves.RemoveAt(index);
            Cell otherCell = GetCell(randomMove.destination.x, randomMove.destination.y);

            if (randomMove.cell.hasMoved) continue;
            if (otherCell.hasMoved) continue;
            if (!randomMove.cell.CanSwapWith(GetCell(randomMove.destination.x, randomMove.destination.y))) continue;

            randomMove.cell.hasMoved = true;
            SwapCells(randomMove.cell.position, randomMove.destination);
        }
    }

    public void TryMove(Cell first, Vector2Int destination)
    {
        // if (!GetCell(destination.x, destination.y).hasMoved)
        // SwapCells(first.position, destination);
        cellMoves.Add(new CellMove
        {
            cell = first,
            destination = destination
        });
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

    public Cell GetCellByIndex(int i) {
        return GetCell(i % width, i / width);
    }

    public void SetCell(int x, int y, Cell cell)
    {
        if (x < 0 || y < 0 || x >= width || y >= height)
        {
            return;
        }

        Cell oldCell = GetCell(x, y);
        if (oldCell != null && oldCell.Solid)
        {
            solidCells.Remove(oldCell);
        }

        cells[x, y] = cell;
        cell.position = new Vector2Int(x, y);

        if (cell.Solid)
        {
            solidCells.Add(cell);
        }
    }

    public void SetElement(int x, int y, Element element)
    {
        Vector2Int position = new Vector2Int(x, y);
        Cell cell = new Cell(element, position);
        SetCell(x, y, cell);
    }
}

public struct CellMove
{
    public Cell cell;
    public Vector2Int destination;
}
