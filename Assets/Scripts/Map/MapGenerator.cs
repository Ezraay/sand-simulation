using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Vector2Int size = new Vector2Int(10, 10);

    public Map Generate()
    {
        Assert.IsTrue(size.x > 0);
        Assert.IsTrue(size.y > 0);

        Map map = new Map(size.x, size.y);

        return map;
    }
}
