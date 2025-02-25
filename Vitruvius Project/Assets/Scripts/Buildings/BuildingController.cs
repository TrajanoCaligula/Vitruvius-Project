using System;
using UnityEngine;

public class BuildingController : MonoBehaviour
{ 
    private float fromLastClick = -1f;
    private float doubleClickRange = 0.3f;

    private bool constructionComplete = false;

    public string MATERIAL_PRODUCTION = "Stone";
    public Inventory[] inputInventory;     // Make an array for inputs and an array for outputs
    public Inventory[] outputInventory;     

    void Start()
    {
        /*
        inventory[].push(new Inventory());
        if (inventory[0] == null)
        {
            Debug.LogError("Inventory es null en HumanController.Start.");
            return; // Detiene la ejecución si inventory es null
        }
        else
        {
            inventory[0].setItemFromDB(MATERIAL_PRODUCTION);
        }*/
    }

    void Update()
    {
        if (constructionComplete)
        {
            
        }
    }

    private void checkConstructionProcess()
    {
        // TODO
        throw new NotImplementedException();
    }

    public void OnMouseDown()
    {
        //TODO: One click, open information of the object TAB

        // Double click, follow the object with the camera
        if (Time.time - fromLastClick < doubleClickRange)
        {
            // Double clicked
            CameraController.instance.followTransform = transform;
        }

        fromLastClick = Time.time;
    }

    public void destroyBuilding()
    {
        //TODO
        Destroy(this);
    }

    // Getters
    public bool isStorageFull() => true;
}
