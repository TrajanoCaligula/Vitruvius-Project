using System;
using UnityEngine;

// TODO: disable the collision with the objectPreview

public class BuildingManager : MonoBehaviour
{
    public GameObject[] objectsList;
    private GameObject objectPreview;
    public BuildingController buildingController;

    private Vector3 targetPosition;
    private RaycastHit hitInfo;
    [SerializeField] private LayerMask layerMask;
    public float rayMaxDistance = 1000;

    public bool snapIsActive = false;
    private bool rotatingObject = false;
    private Vector3 rotateStartPosition;
    private Vector3 rotateCurrentPosition;
    public float rotationSpeed = 5f;
    public float maxRotationSpeed = 5f;
    public float angleSnap = 10;
    [SerializeField] private float mouseSensitivity = 5f; // Factor de escala para suavizar el movimiento del mouse

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If we selected an object to build
        if (objectPreview != null)
        {
            // Lock the position if we are rotating the building
            if (!rotatingObject) objectPreview.transform.position = targetPosition;

            rotateBuilding();

            // Right click to confirm construction
            if (availablePosition() && Input.GetMouseButtonDown(0))
            {
                confirmObject();
            }
        }

        else if (Input.GetKey(KeyCode.J)) selectedObject(0);
        else if (Input.GetKey(KeyCode.K)) selectedObject(1);
        else if (Input.GetKey(KeyCode.L)) selectedObject(2);

        if (Input.GetKey(KeyCode.Escape) || Input.GetMouseButton(1)) selectedObject(-1);
        if (Input.GetKeyDown(KeyCode.LeftAlt)) snapIsActive = !snapIsActive;
    }


    // TODO: Check the correct positioning of the building, checks the layers of both the realworld and building preview layers of the prefab
    private bool availablePosition()
    {
        return true;
    }

    // Checks the position where the mouse is in reference to the terrain (thanks to layerMask)
    private void FixedUpdate()
    {
        Ray destinyPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(destinyPosition, out hitInfo, rayMaxDistance, layerMask))
        {
            targetPosition = hitInfo.point;
        }
    }

    // Building selector based on the index of the buildingsList
    // Index = -1 for destroying preview
    public void selectedObject(int index)
    {
        Destroy(objectPreview);
        objectPreview = null;
        if (index >= 0 && index < objectsList.Length) objectPreview = Instantiate(objectsList[index], targetPosition, transform.rotation);
    }

    // Places the object into the world
    private void confirmObject()
    {
        objectPreview.transform.position = targetPosition;
        objectPreview = null;
    }

    private float currentRotation = 0f; // Almacena la rotación acumulada

    private void rotateBuilding()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            rotatingObject = true;
            rotateStartPosition = Input.mousePosition;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            rotateCurrentPosition = Input.mousePosition;
            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            // Reducimos la sensibilidad y limitamos la velocidad
            float rotationDelta = (difference.x / mouseSensitivity) * rotationSpeed;
            rotationDelta = Mathf.Clamp(rotationDelta, -maxRotationSpeed, maxRotationSpeed);

            if (snapIsActive)
            {
                // Acumulamos la rotación y la ajustamos al snap más cercano
                currentRotation += rotationDelta;
                float snappedRotation = Mathf.Round(currentRotation / angleSnap) * angleSnap;
                objectPreview.transform.rotation = Quaternion.Euler(0, snappedRotation, 0);
            }
            else
            {
                // Aplicamos la rotación con el límite de velocidad
                objectPreview.transform.Rotate(Vector3.up * rotationDelta);
            }

            rotateStartPosition = Input.mousePosition;
        }
        else
        {
            rotatingObject = false;
        }
    }
}
