using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPainter : MonoBehaviour
{
    [SerializeField] private int brushDiameter = 3;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private Element element;
    [SerializeField] private Element eraseElement;

    public void Paint(Vector3 point)
    {
        Fill(point, element);
    }

    public void Erase(Vector3 point)
    {
        Fill(point, eraseElement);
    }

    private void Fill(Vector3 point, Element element)
    {
        Vector2Int position = new Vector2Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y));
        Vector2Int mapHalfSize = mapManager.MapSize / 2;
        Vector2Int halfSize = Vector2Int.one * ((brushDiameter - 1) / 2);
        bool[,] brush = Brushes.SquareBrush(brushDiameter);

        for (int y = 0; y < brush.GetLength(1); y++)
        {
            for (int x = 0; x < brush.GetLength(0); x++)
            {
                Vector2Int cellPosition = new Vector2Int(x, y) + position - halfSize + mapHalfSize;
                Cell cell = new Cell(element, cellPosition);
                mapManager.SetCell(cellPosition.x, cellPosition.y, cell);
            }
        }
    }
}
