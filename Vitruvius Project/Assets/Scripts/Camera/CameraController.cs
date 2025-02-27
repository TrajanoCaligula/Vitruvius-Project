using UnityEngine;

/*
 * TODO: Check comments && terrain to stablish cam limits
 */
public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    Terrain terrain;

    public Transform cameraTransform;
    public Transform followTransform;

    // Velocidad de Movimiento
    public float normalSpeed = 0.5f; // Velocidad normal de movimiento
    public float fastSpeed = 2f; // Velocidad r�pida al presionar Shift
    public float movementTime = 5f; // Tiempo de interpolaci�n para el movimiento

    // Rotaci�n
    public float rotationAmount = 0.5f; // Cantidad de rotaci�n con teclas Q y E
    public float minPitch = 5f; // �ngulo m�nimo de inclinaci�n
    public float maxPitch = 60f; // �ngulo m�ximo de inclinaci�n

    // Zoom
    public float minZoom = 2f; // Distancia m�nima de la c�mara
    public float maxZoom = 500f; // Distancia m�xima de la c�mara
    private Vector3 zoomAmount = new Vector3(0, 1, 1); // Cantidad de zoom

    private float movementSpeed; // Velocidad de movimiento actual
    private Vector3 newPosition; // Nueva posici�n de la c�mara
    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;
    private Vector3 rotateStartPosition;
    private Vector3 rotateCurrentPosition;

    private float pitch = 30f;  // Inclinaci�n vertical
    private float yaw = 0f;     // Rotaci�n horizontal
    private float cameraDistance; // Distancia de la c�mara al Camera Rig

    void Start()
    {
        terrain = Terrain.activeTerrain;
        instance = this;
        newPosition = transform.position;

        yaw = transform.rotation.eulerAngles.y;
        pitch = 30f; // �ngulo inicial de inclinaci�n

        // Get main camera from gameobject (error control)
        cameraTransform = Camera.main.transform;

        // Se calcula la distancia inicial de la c�mara al camera rig
        cameraDistance = Vector3.Distance(cameraTransform.position, transform.position);
    }

    void Update()
    {
        if (followTransform != null)
        {
            transform.position = Vector3.Lerp(transform.position, followTransform.position, Time.deltaTime * movementTime);
            newPosition = transform.position;
            if (Input.GetKeyDown(KeyCode.Escape)) followTransform = null;
        } else
        {

        }

        HandleMovementInput();
        HandleRotationInput();
        HandleZoomInput();
    }

    void HandleMovementInput()
    {
        // Mouse Change for screen limit movement
        /*
         * TODO: When mouse close to the viewport limit, move towards that direction
         */

        // KeyBoard
        movementSpeed = Input.GetKey(KeyCode.LeftShift) ? fastSpeed : normalSpeed;

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

            yaw -= difference.x * 0.2f;
            pitch += difference.y * 0.2f;

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        // KeyBoard
        if (Input.GetKey(KeyCode.Q))
            yaw += rotationAmount;
        if (Input.GetKey(KeyCode.E))
            yaw -= rotationAmount;

        // Aplicar la rotaci�n en yaw (horizontal) al Camera Rig
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, yaw, 0), Time.deltaTime * movementTime);

        // Calcular la nueva posici�n de la c�mara en base al pitch
        float pitchRadians = pitch * Mathf.Deg2Rad;
        Vector3 cameraOffset = new Vector3(0, Mathf.Sin(pitchRadians) * cameraDistance, -Mathf.Cos(pitchRadians) * cameraDistance);

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraOffset, Time.deltaTime * movementTime);

        // Ajustar la rotaci�n de la c�mara para que mire hacia el centro del arco
        Quaternion targetRotation = Quaternion.Euler(pitch, 0, 0);
        cameraTransform.localRotation = Quaternion.Lerp(cameraTransform.localRotation, targetRotation, Time.deltaTime * movementTime);
    }

    void HandleZoomInput()
    {
        // Mouse
        if (Input.mouseScrollDelta.y != 0)
        {
            cameraDistance -= Input.mouseScrollDelta.y * zoomAmount.z * 10;
        }

        // KeyBoard
        if (Input.GetKey(KeyCode.Z))
        {
            cameraDistance -= zoomAmount.z;
        }
        if (Input.GetKey(KeyCode.X))
        {
            cameraDistance += zoomAmount.z;
        }

        cameraDistance = Mathf.Clamp(cameraDistance, minZoom, maxZoom);

        // Se actualiza la posici�n de la c�mara manteniendo su �ngulo
        float pitchRadians = pitch * Mathf.Deg2Rad;
        Vector3 cameraOffset = new Vector3(0, Mathf.Sin(pitchRadians) * cameraDistance, -Mathf.Cos(pitchRadians) * cameraDistance);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraOffset, Time.deltaTime * movementTime);
    }
}
