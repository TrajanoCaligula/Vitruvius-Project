using System;
using UnityEngine;

public class BuildingController : MonoBehaviour
{ 
    private float fromLastClick = -1f;
    private float doubleClickRange = 0.3f;

    private bool constructionComplete = false;

    void Start()
    {

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
}
