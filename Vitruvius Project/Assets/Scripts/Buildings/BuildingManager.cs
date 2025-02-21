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
        if (objectPreview != null)
        {
            if (!rotatingObject) objectPreview.transform.position = targetPosition;

            rotateBuilding();

            if (Input.GetMouseButtonDown(0))
            {
                confirmObject();
            }
        }
        else if (Input.GetKey(KeyCode.Alpha1)) selectedObject(0);
        else if (Input.GetKey(KeyCode.Alpha2)) selectedObject(1);
        else if (Input.GetKey(KeyCode.Alpha3)) selectedObject(2);

        if (Input.GetKey(KeyCode.Escape) || Input.GetMouseButton(1)) selectedObject(-1);
        if (Input.GetKeyDown(KeyCode.LeftAlt)) snapIsActive = !snapIsActive;
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

    //Object selector from outside the script (for the UI)
    public void selectedObject(int index)
    {
        if (index >= 0) objectPreview = Instantiate(objectsList[index], targetPosition, transform.rotation);
        else
        {
            // TODO: Destory the object Preview 
            objectPreview = null;
        }
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
