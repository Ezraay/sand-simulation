using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Color Colour => colour;

    private Element element;
    private Color colour;

    public Cell(Element element)
    {
        this.element = element;
        this.colour = element.Colour;

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
}
