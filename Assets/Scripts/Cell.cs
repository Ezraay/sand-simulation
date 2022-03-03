using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Color Colour => colour;
    public float Density => element.Density;

    public bool hasMoved;
    public bool movedLastTick = false;

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

    public void Tick(Neighbours neighbours, Map map)
    {
        bool movingLeft = Random.Range(0, 1) == 0;
        Cell[] updateOrder = new Cell[] {
            neighbours.bottom,
            movingLeft ? neighbours.bottomLeft : neighbours.bottomRight,
            movingLeft ? neighbours.bottomRight : neighbours.bottomLeft,
            element.LiquidMovementType ? (movingLeft ? neighbours.left : neighbours.right) : null,
            element.LiquidMovementType ? (movingLeft ? neighbours.right : neighbours.left) : null,
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
