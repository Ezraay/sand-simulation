using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private MapGenerator generator;
    [SerializeField] private new MapRenderer renderer;

    public Vector2Int MapSize => new Vector2Int(map.width, map.height);
    private bool paused = false;

    private Map map;

    private void Start()
    {
        map = generator.Generate();
    }

    private void FixedUpdate()
    {
        if (!paused)
        {
            map.Tick();
        }


        renderer.Render(map);
    }

    public void TogglePause()
    {
        paused = !paused;
    }

    public void SetCell(int x, int y, Cell cell)
    {
        map.SetCell(x, y, cell);
    }
}
