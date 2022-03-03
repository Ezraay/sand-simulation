using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(SpriteRenderer))]
public class MapRenderer : MonoBehaviour
{
    private new SpriteRenderer renderer;
    private float pixelsPerUnit = 1f;
    private Vector2 pivot = new Vector2(0.5f, 0.5f);

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void Render(Map map)
    {
        Assert.IsNotNull(map);

        if (renderer.sprite != null)
        {
            Destroy(renderer.sprite.texture);
            Destroy(renderer.sprite);
        }

        Texture2D texture = new Texture2D(map.width, map.height);
        Color[] colours = new Color[map.width * map.height];
        for (int y = 0; y < map.height; y++)
        {
            for (int x = 0; x < map.width; x++)
            {
                Cell cell = map.GetCell(x, y);
                int index = x + y * map.width;
                colours[index] = cell == null ? Color.white : cell.Colour;
            }
        }
        texture.SetPixels(colours);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();

        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Sprite sprite = Sprite.Create(texture, rect, pivot, pixelsPerUnit);
        renderer.sprite = sprite;
    }
}
