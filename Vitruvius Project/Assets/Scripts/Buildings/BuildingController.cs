using System;
using UnityEngine;

public class BuildingController : MonoBehaviour
{ 
    private float fromLastClick = -1f;
    private float doubleClickRange = 0.3f;

    private bool constructionComplete = false;

    // WorkPlace
    public int numberOfWorkers = 0;
    public int maxNumberOfWorkers = 5;

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

        if (Input.GetKey(KeyCode.Escape)) UIManager.instance.ocultarPanelNegro();

    }

    private void checkConstructionProcess()
    {
        // TODO
        throw new NotImplementedException();
    }

    public void OnMouseDown()
    {
        //TODO: One click, open information of the object TAB
        UIManager.instance.mostrarPanelNegro();
        UIManager.instance.updateBuilding(this.gameObject);
        UIManager.instance.updateWorkers(numberOfWorkers);

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
        UIManager.instance.updateBuilding(this.gameObject);
        Destroy(this);
    }

    // Setters
    public void addWorker()
    {
        if (numberOfWorkers < maxNumberOfWorkers) numberOfWorkers++;
    }

    public void subWorker()
    {
        if (numberOfWorkers > 0) numberOfWorkers--;
    }

    // Getters
    public bool isStorageFull() => true;

    public int getNumberWorkers() => numberOfWorkers;
}
