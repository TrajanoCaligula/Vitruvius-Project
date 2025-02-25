using UnityEngine;

public class ResourceNode : MonoBehaviour
{

    public Inventory inventory;
    public string resourceName = "Stone";
    public int quantity = 1000;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void mineNode()
    {
        quantity--;
        //if (quantity <= 0) DestroyNode();
    }
}
