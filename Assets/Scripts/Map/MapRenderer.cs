using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

[RequireComponent(typeof(SpriteRenderer))]
public class MapRenderer : MonoBehaviour
{
    [SerializeField] private RenderMode renderMode;
    [SerializeField] private Color firstColour = Color.red;
    [SerializeField] private Color secondColour = Color.green;

    private enum RenderMode
    {
        Element, Strength, Weight
    }
    private delegate Texture2D RendererFunction(Map map);
    private Dictionary<RenderMode, RendererFunction> renderFunctions;

    private new SpriteRenderer renderer;
    private float pixelsPerUnit = 1f;
    private Vector2 pivot = new Vector2(0.5f, 0.5f);

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderFunctions = new Dictionary<RenderMode, RendererFunction>() {
        {RenderMode.Element, this.ElementMode},
        // {RenderMode.Strength, this.SupportMode},
        // {RenderMode.Weight, this.WeightMode}
    };
    }

    public void Render(Map map)
    {
        Assert.IsNotNull(map);

        if (renderer.sprite != null)
        {
            Destroy(renderer.sprite.texture);
            Destroy(renderer.sprite);
        }

        Texture2D texture = renderFunctions[renderMode](map);

        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Sprite sprite = Sprite.Create(texture, rect, pivot, pixelsPerUnit);
        renderer.sprite = sprite;
    }

    private Texture2D ElementMode(Map map)
    {
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

        return texture;
    }

    // private Texture2D SupportMode(Map map)
    // {
    //     Texture2D texture = new Texture2D(map.width, map.height);
    //     Color[] colours = new Color[map.width * map.height];
    //     Cell firstCell = map.GetCell(0, 0);
    //     float minSupport = firstCell.support;
    //     float maxSupport = firstCell.support;

    //     for (int y = 0; y < map.height; y++)
    //     {
    //         for (int x = 0; x < map.width; x++)
    //         {

    //             Cell cell = map.GetCell(x, y);
    //             minSupport = Mathf.Min(minSupport, cell.support);
    //             maxSupport = Mathf.Max(maxSupport, cell.support); 
    //         }   
    //     }

    //     for (int y = 0; y < map.height; y++)
    //     {
    //         for (int x = 0; x < map.width; x++)
    //         {
    //             Cell cell = map.GetCell(x, y);
    //             float range = maxSupport - minSupport;
    //             float support = cell.support - minSupport;
    //             float result = support / range;
    //             // float support = Mathf.Lerp(0f, 1f, Mathf.InverseLerp(minSupport, maxSupport, cell.support));
                
    //             // Debug.Log(result);
    //             int index = x + y * map.width;
    //             colours[index] = Color.Lerp(firstColour, secondColour, result);
    //         }
    //     }

    //     texture.SetPixels(colours);
    //     texture.filterMode = FilterMode.Point;
    //     texture.wrapMode = TextureWrapMode.Clamp;
    //     texture.Apply();

    //     return texture;
    // }

    // private Texture2D WeightMode(Map map)
    // {
    //     Texture2D texture = new Texture2D(map.width, map.height);
    //     Color[] colours = new Color[map.width * map.height];
    //     Cell firstCell = map.GetCell(0, 0);
    //     float minWeight = firstCell.support;
    //     float maxWeight = firstCell.support;

    //     for (int y = 0; y < map.height; y++)
    //     {
    //         for (int x = 0; x < map.width; x++)
    //         {
    //             Cell cell = map.GetCell(x, y);
    //             minWeight = Mathf.Min(minWeight, cell.weight);
    //             maxWeight = Mathf.Max(maxWeight, cell.weight);
    //         }
    //     }

    //     for (int y = 0; y < map.height; y++)
    //     {
    //         for (int x = 0; x < map.width; x++)
    //         {
    //             Cell cell = map.GetCell(x, y);
    //             float weight = (cell.weight - minWeight) / (maxWeight - minWeight);
    //             int index = x + y * map.width;
    //             colours[index] = Color.Lerp(firstColour, secondColour, weight);
    //         }
    //     }

    //     texture.SetPixels(colours);
    //     texture.filterMode = FilterMode.Point;
    //     texture.wrapMode = TextureWrapMode.Clamp;
    //     texture.Apply();

    //     return texture;
    // }
}
