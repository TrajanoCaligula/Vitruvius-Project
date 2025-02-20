using UnityEngine;
using UnityEngine.AI;

public class HumanController : MonoBehaviour
{
    public NavMeshAgent agent;

    private float fromLastClick = -1f;
    private float doubleClickRange = 0.3f; 

    void Start()
    {

    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray destinyPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(destinyPosition, out var hitInfo))
            {
                agent.SetDestination(hitInfo.point);
            }
        }
    }

    public void OnMouseDown()
    {
        if (Time.time - fromLastClick < doubleClickRange)
        {
            // Double clicked
            CameraController.instance.followTransform = transform;
        }

        fromLastClick = Time.time;
    }
}
