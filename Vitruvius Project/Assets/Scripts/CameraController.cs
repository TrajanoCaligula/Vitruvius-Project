using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    Terrain terrain;

    public Transform cameraTransform;
    public Transform followTransform;

    public float normalSpeed;
    public float fastSpeed;
    private float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    public float distanceFromTerrain = 5f;

    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;
    private Vector3 rotateStartPosition;
    private Vector3 rotateCurrentPosition;

    private Vector3 newPosition;
    private Quaternion newRotation;
    private Vector3 newZoom;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        terrain = Terrain.activeTerrain;
        instance = this;
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (followTransform != null)
        {
            transform.position = Vector3.Lerp(transform.position, followTransform.position, Time.deltaTime * movementTime);
            if (Input.GetKeyDown(KeyCode.Escape)) followTransform = null;
        }

        HandleMovementInput();
        HandleRotationInput();
        HandleZoomInput();
    }

    void HandleMovementInput()
    {
        // Mouse Change for screen limit movement
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }
        // KeyBoard
        if (Input.GetKey(KeyCode.LeftShift)) movementSpeed = fastSpeed;

        else movementSpeed = normalSpeed;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
            followTransform = null;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
            followTransform = null;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
            followTransform = null;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
            followTransform = null;
        }

        if (followTransform == null) transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }
    void HandleRotationInput()
    {
        // Mouse
        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }
        // KeyBoard
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
    }

    void HandleZoomInput()
    {
        // Mouse
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount * 10;
        }
        // KeyBoard
        if (Input.GetKey(KeyCode.Z))
        {
            newZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.X))
        {
            newZoom -= zoomAmount;
        }

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}
