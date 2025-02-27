using System;
using UnityEngine;
using UnityEngine.AI;

public class HumanController : MonoBehaviour
{

    public enum State
    {
        Work,   // State for working time
        Rest,   // State for resting time
        Idle,   // State for work or rest when one or the other is null
        Buy,    // State for market buying
        Pray,   // State for temple assistance
        Free    // State for free time
    }

    /*
    public enum SocialClass // USE TAGS?
    {
        Clientes,   // Clients 
        Servi,      // Slaves
        Liberti,    // FreedMen
        Equites,    // Chivalry
        Plebeii,    // Plebeians
        Patricii    // Patricians
    }
    */
    public NavMeshAgent agent;
    public State state;
    public State nextState;

    // Main variables
    public GameObject home = null;
    public GameObject workPlace = null;

    // Inventory
    public int MAX_INVENTORY_QTY = 5;

    // Jobs System
    public Inventory inventory;
    public IJobsSystem job;
    public GameObject childGameObject;

    // Click timeing
    private float fromLastClick = -1f;
    private float doubleClickRange = 0.3f;

    // State machine
    public float timerLastChange;
    private float timerStateChange = 100f; // In seconds
    public bool endCurrentTask = false;

    void Start()
    {
        // TESTING
        state = State.Free;
        nextState = state;
        timerLastChange = 0f;
        agent.acceleration = 10000f;        // Makes the speed change instantly

        //Suscribe to events
        TimeManager.Instance.EventCircus += freeState;
        TimeManager.Instance.EventCircus += prayState;

        // Inventory
        inventory = new Inventory(); // Asegúrate de que esto se ejecute
        if (inventory == null)
        {
            Debug.LogError("Inventory es null en HumanController.Start.");
            return; // Detiene la ejecución si inventory es null
        }
        else
        {
            Debug.Log(inventory.itemName +" "+ inventory.itemQty + " "+ inventory.itemMaxQty);
        }
        
    }

    void Update()
    {
        if (TimeManager.Instance != null && !TimeManager.Instance.isGamePaused())
        {
            // Time control
            timerLastChange += TimeManager.Instance.getDeltaTime();
            if (timerLastChange >= timerStateChange) changeState();


            if (Input.GetMouseButtonDown(1))
            {
                Ray destinyPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(destinyPosition, out var hitInfo))
                {
                    agent.SetDestination(hitInfo.point);
                }
            }
            switch (state)
            {
                case State.Idle:
                    break;

                case State.Rest:
                    restState();
                    break;

                case State.Buy:
                    buyState();
                    break;

                case State.Work:
                    workState();
                    break;

                case State.Pray:
                    prayState();
                    break;

                case State.Free:
                    freeState();
                    break;
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

    public void changeState()
    {
        nextState = (State)(((int)state + 1) % State.GetValues(typeof(State)).Length);
        // Change state if citizens ended their task
        if (!endCurrentTask)
        {
            timerLastChange = 0f;
            state = nextState;

            agent.ResetPath();
        }
    }

    // Jobs
    public void workState()
    {
        // If has no job go to City Center
        if (workPlace == null)
        {
            if (!agent.hasPath) agent.SetDestination(CityCenterScript.Instance.getRandomPositionAround());
        }
        else
        {
            // Job not started: go to the job
            if (!job.isActive())
            {
                if (!agent.hasPath) agent.SetDestination(workPlace.transform.position);
                if (getDistanceXZ(transform.position, workPlace.transform.position) <= job.getMinDistance()) job.startShift();
            }
            // Keep working
            else
            {
                job.working();
            }

        }
    }

    public void addJobAndScript()
    {
        GameObject newJob = new GameObject("Job");
        newJob.transform.SetParent(transform);

        string type = workPlace.GetComponent<BuildingController>().MATERIAL_PRODUCTION;

        if (string.IsNullOrEmpty(type))
        {
            Debug.LogError("MATERIAL_PRODUCTION is empty or null.");
            Destroy(newJob); // Cleanup created GameObject
            return; // Exit if MATERIAL_PRODUCTION is invalid
        }

        switch (type)
        {
            case "Stone":
                // Sets miner and changes the citizine from uneployed to employed list
                setMiner(newJob, type);
                break;
            default:
                Debug.LogWarning($"Unknown material type: {type}. No job created.");
                Destroy(newJob); // Cleanup created GameObject
                break;
        }
    }

    public void setMiner(GameObject jobObject, string type)
    {
        Miner minerComponent = jobObject.AddComponent<Miner>();
        minerComponent.setType(type);

        job = minerComponent;
        GameManager.instance.fromUnemployedToEmployed(this.gameObject);
        workPlace.GetComponent<BuildingController>().addWorker();
    }

    // Housing
    public void restState()
    {
        if (home == null)
        {
            if (findHome()) agent.SetDestination(home.transform.position);
            else if (!agent.hasPath) agent.SetDestination(CityCenterScript.Instance.getRandomPositionAround());
        }
    }


    public bool findHome()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("House");
        
        foreach (GameObject gameObject in taggedObjects)
        {
            home = gameObject;
            return true;
        }
        return false;
    }

    public bool findBuilding(string Tag)
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(Tag);

        foreach (GameObject gameObject in taggedObjects)
        {
            if (Tag == "House") home = gameObject;
            return true;
        }
        return false;
    }

    public Vector3 findNeed(string Tag)
    {
        // Busca todos los GameObjects que tengan la etiqueta especificada
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(Tag);

        // Itera sobre los objetos encontrados y haz algo con ellos
        foreach (GameObject gameObject in taggedObjects)
        {
            return gameObject.transform.position;
            // Puedes acceder a los componentes del objeto aquí, por ejemplo:
            // objeto.GetComponent<MiComponente>().HacerAlgo();
        }
        return new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity); ;
    }

    public void buyState()
    {
        // Check needs
        agent.SetDestination(findNeed("Well"));
    }

    private void freeState()
    {
        throw new NotImplementedException();
    }

    private void prayState()
    {
        throw new NotImplementedException();
    }

    // Distance only between axis x and z
    public static float getDistanceXZ(Vector3 a, Vector3 b)
    {

        float diffX = Mathf.Abs(a.x - b.x);
        float diffZ = Mathf.Abs(a.z - b.z);

        float distance = Mathf.Sqrt(diffX * diffX + diffZ * diffZ);

        return distance;
    }

    // Getters
    public State getState()
    {
        return state;
    }

    // Setters

    public void setState(State newState)
    {
        state = newState;
    }

}
