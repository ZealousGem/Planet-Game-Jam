using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : GameplayInputBinding
{
    /// <summary>
    /// 
    /// This script handles all the player movement such as zooms and moving the camera  
    /// 
    /// 
    /// </summary>
    private Camera cam;

    [Header("Camera Speed")]
    [SerializeField] private float speed = 10f;

    [Header("Radius Properties")]
    [SerializeField] private float minX = -10, MaxX = 70;
    [SerializeField] private float minY = -30, MaxY = 40;

    [Header("Camera Zoom Properties")]
    [SerializeField] float MaxZoom = 8.07f; // allows camera to zoom 
    [SerializeField] float MinZoom = 13.73183f;
    [SerializeField] float ZoomOut = 18f;
    [SerializeField] float ZoomSpeed = 4f;
    private Vector3 movement;
    private bool isZoomedIn = false;

    private bool isZoomedOut = false;

    private float currentZoomSize;

    private Coroutine ZoomEffect;

    // binding camera
    protected override void Awake()
    {
        base.Awake();
        cam = GetComponent<Camera>();
    }

    // inputs for movement 
    protected override void MoveSatelite(InputAction.CallbackContext value) => movement = value.ReadValue<Vector2>();
    protected override void CancelMovment(InputAction.CallbackContext value) => movement = Vector2.zero;

    // zoom in camera function that will zoom to a certain size based on whether the bool is true or false
    protected override void ZoomCamera(InputAction.CallbackContext value)
    {
        if (isZoomedIn) isZoomedOut = false;

        if (!isZoomedIn)
        {
            isZoomedIn = true;
            currentZoomSize = MaxZoom;
            if (isZoomedOut) isZoomedOut = false;
        }

        else
        {
            isZoomedIn = false;
            currentZoomSize = MinZoom;
        }

        StartCouritineForZoom();
    }

    // zoomout function similar to the zoom in function but making the zoom further out
    protected override void ZoomOutCamera(InputAction.CallbackContext value)
    {
        if (!isZoomedOut)
        {
            isZoomedOut = true;
            currentZoomSize = ZoomOut;
        }

        else
        {
            isZoomedOut = false;
            currentZoomSize = MinZoom;
            if (isZoomedIn) isZoomedIn = false;
        }

        StartCouritineForZoom();

    }

    // this helps make sure courtines are not overloaded and cleaning stops the courtine if and input is immedaly invoked
    private void StartCouritineForZoom()
    {
        if (ZoomEffect != null)
        {
            StopCoroutine(ZoomEffect);
            ZoomEffect = null;
        }

        ZoomEffect = StartCoroutine(ZoomCameraEffect(currentZoomSize));
    }
    // pauses game and disable player controls, might remove it and ad this action in the pause menu  
    protected override void PauseGame(InputAction.CallbackContext value)
    {

    }

    // moves the camera

    private void LateUpdate() => HandleMovement();

    // handles zoom transition 
    private IEnumerator ZoomCameraEffect(float targetSize)
    {
        if (cam == null)
        {
            Debug.Log("Camera not instantied");
            yield break;
        }

        while (!Mathf.Approximately(cam.orthographicSize, targetSize))
        {
            cam.orthographicSize = Mathf.MoveTowards(
            cam.orthographicSize,
            targetSize,
            ZoomSpeed * Time.deltaTime);

            yield return null;
        }

        cam.orthographicSize = targetSize;
        ZoomEffect = null;
    }

    // handles movement offset 
    private void HandleMovement()
    {
        transform.position += movement * speed * Time.deltaTime;

        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minX, MaxX);
        clampedPos.y = Mathf.Clamp(clampedPos.y, minY, MaxY);
        transform.position = clampedPos;
    }
}
