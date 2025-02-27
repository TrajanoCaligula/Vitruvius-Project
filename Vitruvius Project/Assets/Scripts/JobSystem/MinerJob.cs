using UnityEngine;
using UnityEngine.AI;

public class Miner : MonoBehaviour, IJobsSystem
{
    private HumanController humanController;
    public string resourceName;
    [SerializeField] private float mineTime = 10f;
    [SerializeField] private float minDistanceBuilding = 7.5f;
    private ResourceNode resourceNode;
    private float counter = 0f;
    [SerializeField] private bool isWorking = false;

    private void Start()
    {
        humanController = GetComponentInParent<HumanController>();
        if (humanController == null)
        {
            Debug.LogError("No HumanController found in parent!");
            return;
        }
    }

    public void startShift()
    {
        isWorking = true;
        humanController.inventory.setItemFromDB(resourceName);
    }

    public void working()
    {
        if (humanController.inventory.isFull() || humanController.state != humanController.nextState)
        {
            handleReturnToWorkplace();
            // Prevents citizen to change the stat before ending task
            if (humanController.state != humanController.nextState) stopShift();
        }
        else
        {
            mineResource();
        }
    }

    public void stopShift()
    {
        Debug.Log("Shift ended");
        isWorking = false;
    }

    private void handleReturnToWorkplace()
    {
        if (HumanController.getDistanceXZ(humanController.workPlace.transform.position, humanController.transform.position) <= minDistanceBuilding)
        {
            downloadInventory();
            if (humanController.endCurrentTask)
            {
                stopShift();
            }
        }
        else
        {
            humanController.agent.SetDestination(humanController.workPlace.transform.position);
        }
    }

    private void mineResource()
    {
        if (resourceNode == null)
        {
            findNearestResource();
            if (resourceNode == null) return;
        }

        if (humanController.agent.remainingDistance <= minDistanceBuilding / 5)
        {
            humanController.agent.ResetPath();
            mineNode();
        }
        else
        {
            humanController.agent.SetDestination(resourceNode.transform.position);
        }
    }

    private void findNearestResource()
    {
        GameObject resourceNodeObject = GameObject.FindGameObjectWithTag("ResourceNode");

        if (resourceNodeObject != null)
        {
            resourceNode = resourceNodeObject.GetComponent<ResourceNode>();
        }
        else
        {
            Debug.LogWarning("No ResourceNode found.");
            resourceNode = null;
        }
    }

    private void mineNode()
    {
        counter += TimeManager.Instance.getDeltaTime();
        if (counter >= mineTime)
        {
            Debug.Log("Mined");
            humanController.endCurrentTask = true;
            counter = 0f;
            resourceNode.mineNode();
            humanController.inventory.AddOneItem(resourceName);
            Debug.Log(humanController.inventory.itemQty);
        }
    }

    private void downloadInventory()
    {
        humanController.endCurrentTask = false;
        humanController.inventory.emptyInventory();
    }

    // Getters
    public bool isActive() => isWorking;
    public float getMinDistance() => minDistanceBuilding;

    // Setters
    public void setType(string resourceName) => this.resourceName = resourceName;

}