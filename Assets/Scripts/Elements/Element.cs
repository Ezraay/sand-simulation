using UnityEngine;

[CreateAssetMenu(fileName = "New Element", menuName = "New Element", order = 0)]
public class Element : ScriptableObject {
    public string Name => name;
    public Color32 Colour => colour;
    public float Density => density;
    public bool Solid => solid;
    public float Strength => strength;
    public float Weight => weight;
    
    [SerializeField] private new string name = "New element";
    [SerializeField] private Color32 colour = Color.white;

    [SerializeField] private float density = 1f; // Determines whether an element sinks below another, kg/m3
    [SerializeField] private bool solid;
    
    [SerializeField] private float strength = 0; // Determines how strong an element is at holding weight above it
    [SerializeField] private float weight = 1f; // Determines how much this tile affects the structure of those below
}