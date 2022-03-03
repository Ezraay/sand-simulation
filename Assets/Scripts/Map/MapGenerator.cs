using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Vector2Int size = new Vector2Int(10, 10);
    [SerializeField] private Element mapFill;

    public Map Generate()
    {
        Assert.IsTrue(size.x > 0);
        Assert.IsTrue(size.y > 0);
        Assert.IsNotNull(mapFill);

        Map map = new Map(size.x, size.y);
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                map.SetElement(x, y, mapFill);
            }
        }

        return map;
    }
}
