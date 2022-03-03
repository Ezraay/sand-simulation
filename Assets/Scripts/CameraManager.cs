using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float smoothTime = 10f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 20f;
    [SerializeField] private float zoomStep = 1f;

    private new Camera camera;

    private Vector3 targetPosition;
    private float zoom;

    private Vector2 input;
    private float zoomInput;

    private void Start()
    {
        camera = GetComponent<Camera>();
        
        targetPosition = transform.position;
        zoom = maxZoom;
    }

    private void Update()
    {
        // Movement
        Vector3 velocity = input * movementSpeed * Time.deltaTime;
        targetPosition += velocity;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothTime * Time.deltaTime);

        // Zoom
        zoom += zoomInput * zoomStep;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoom, smoothTime * Time.deltaTime);
    }

    public void SetInput(Vector2 input, float zoom)
    {
        this.input = input;
        this.zoomInput = zoom;
    }
}
