using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : GameplayInputBinding
{

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

    protected override void Awake()
    {
        base.Awake();
        cam = GetComponent<Camera>();
    }
    protected override void MoveSatelite(InputAction.CallbackContext value) => movement = value.ReadValue<Vector2>();
    protected override void CancelMovment(InputAction.CallbackContext value) => movement = Vector2.zero;
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

    private void StartCouritineForZoom()
    {
        if (ZoomEffect != null)
        {
            StopCoroutine(ZoomEffect);
            ZoomEffect = null;
        }

        ZoomEffect = StartCoroutine(ZoomCameraEffect(currentZoomSize));
    }

    protected override void PauseGame(InputAction.CallbackContext value)
    {

    }

    private void LateUpdate() => HandleMovement();


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

    private void HandleMovement()
    {
        transform.position += movement * speed * Time.deltaTime;

        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minX, MaxX);
        clampedPos.y = Mathf.Clamp(clampedPos.y, minY, MaxY);
        transform.position = clampedPos;
    }
}
