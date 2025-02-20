using System;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] objectsList;
    private GameObject objectPreview;

    private Vector3 targetPosition;
    private RaycastHit hitInfo;
    [SerializeField] private LayerMask layerMask;
    public float rayMaxDistance = 1000;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(objectPreview != null)
        {
            objectPreview.transform.position = targetPosition;

            if (Input.GetMouseButtonDown(0))
            {
                confirmObject();
            }
        } else if (Input.GetKey(KeyCode.Alpha1)) selectedObject(0);
    }

    // Places the object into the world
    private void confirmObject()
    {
        objectPreview = null;
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
        objectPreview = Instantiate(objectsList[index], targetPosition, transform.rotation);
    }
}
