using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Color Colour => colour;
    public float Density => element.Density;

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

    public void Darken(float maxAmount = 0.1f)
    {
        float amount = Random.Range(0, maxAmount);
        colour.r += amount;
        colour.g += amount;
        colour.b += amount;
    }

    public void Tick(Neighbours neighbours, Map map)
    {
        if (neighbours.bottom != null && Density > neighbours.bottom.Density) {
            map.SwapCells(this, neighbours.bottom);
        }
    }
}

public struct Neighbours {
    public Cell bottom;
    public Cell top;
    public Cell left;
    public Cell right;

    public Cell bottomLeft;
    public Cell bottomRight;
    public Cell topLeft;
    public Cell topRight;
}
