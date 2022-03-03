using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private MapPainter mapPainter;

    private void Update()
    {
        // Camera movement
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();

        // Camera zoom
        float zoom = -Input.mouseScrollDelta.y;
        cameraManager.SetInput(input, zoom);

        // Point and click
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mapPainter.Paint(mousePosition);
        }

        // Pause
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mapManager.TogglePause();
        }
    }
}
