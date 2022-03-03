using UnityEngine;

[CreateAssetMenu(fileName = "New Element", menuName = "New Element", order = 0)]
public class Element : ScriptableObject {
    public string Name => name;
    public Color32 Colour => colour;
    public float Strength => strength;
    public float Density => density;
    public bool LiquidMovementType => liquidMovementType;
    
    [SerializeField] private new string name = "New element";
    [SerializeField] private Color32 colour = Color.white;

    [SerializeField] private float strength = 0; // Determines how strong an element is at holding weight
    [SerializeField] private float density = 1; // Determines whether an element sinks below another
    [SerializeField] private bool liquidMovementType; // If checked, will move side to side to fill a container
}

// TODO: Future heat stuff
// [System.Serializable]
// public struct ElementTemperatureTransition {
//     public Element element;
//     public float temperature;
// }