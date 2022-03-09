using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Color Colour => colour;
    public float Density => element.Density;
    public bool Solid => element.Solid;
    public float Weight => element.Weight;
    public float Strength => element.Strength;

    public int x => position.x;
    public int y => position.y;

    public bool hasMoved;
    public bool movedLastTick = false;
    public bool structurallySound = false;

    public Vector2Int position;
    private Element element;
    private Color colour;


    public Cell(Element element, Vector2Int position)
    {
        this.element = element;
        this.colour = element.Colour;
        this.position = position;

        if (this.element != null)
        {
            Darken();
        }
    }

    public void Darken(float maxAmount = 0.05f)
    {
        float amount = Random.Range(0, maxAmount);
        colour.r -= amount;
        colour.g -= amount;
        colour.b -= amount;
    }

    // public void CalculateWeight(Map map)
    // {
    //     weight = element.Weight;
    //     for (int y = 1; y <= strengthLookSize; y++)
    //     {
    //         for (int x = -strengthLookSize; x <= strengthLookSize; x++)
    //         {
    //             if (x == 0 && y == 0) continue;

    //             Cell cell = map.GetCell(position.x + x, position.y + y);
    //             if (cell == null) continue;
    //             if (!cell.Solid) continue;

    //             weight += cell.weight;
    //         }
    //     }
    // }

    // public void CalculateSupport(Map map)
    // {
    //     support = element.Strength;
    //     for (int y = -strengthLookSize; y < 0; y++)
    //     {
    //         for (int x = -strengthLookSize; x <= strengthLookSize; x++)
    //         {
    //             if (x == 0 && y == 0) continue;
    //             if (position.y + y < 0)
    //             {
    //                 // Bottom of the map
    //                 support += supportFromBottom;
    //                 continue;
    //             }

    //             Cell cell = map.GetCell(position.x + x, position.y + y);

    //             if (cell == null) continue;
    //             if (!cell.Solid) continue;
    //             support += cell.support;
    //         }
    //     }
    // }

    public void Tick(Neighbours neighbours, Map map)
    {
        if (structurallySound) return;
        // CalculateSupport(map);
        // if (Solid && support >= weight)
        // {
        //     // Transfer weight
        //     return;
        // }

        bool movingLeft = Random.Range(0, 2) == 0;
        Cell[] updateOrder = new Cell[] {
            neighbours.bottom,
            movingLeft ? neighbours.bottomLeft : neighbours.bottomRight,
            movingLeft ? neighbours.bottomRight : neighbours.bottomLeft,
            element.Solid ? null : (movingLeft ? neighbours.left : neighbours.right),
            element.Solid ? null : (movingLeft ? neighbours.right : neighbours.left),
        };

        foreach (Cell cell in updateOrder)
        {
            if (cell != null && Density > cell.Density)
            {
                map.TryMove(this, cell.position);
                break;
            }
        }
    }

    public bool CanSwapWith(Cell cell)
    {
        return cell != null && (this.Density > cell.Density);
    }
}

public struct Neighbours
{
    public Cell bottom;
    public Cell top;
    public Cell left;
    public Cell right;

    public Cell bottomLeft;
    public Cell bottomRight;
    public Cell topLeft;
    public Cell topRight;
}
