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
    private State state;

    public GameObject home = null;
    public GameObject workPlace = null;

    private float fromLastClick = -1f;
    private float doubleClickRange = 0.3f;
    public float timerLastChange;
    private float timerStateChange = 10f; // In seconds

    void Start()
    {
        // TESTING
        state = State.Idle;
        timerLastChange = 0f;
        agent.acceleration = 10000f;        // Makes the speed change instantly

        //Suscribe to events
        TimeManager.Instance.EventCircus+= goToCircus;
        TimeManager.Instance.EventCircus+= goToTemple;

    }

    void Update()
    {
        if (TimeManager.Instance != null && !TimeManager.Instance.isGamePaused())
        {
            // Time control
            timerLastChange += TimeManager.Instance.getDeltaTime();
            if (timerLastChange >= timerStateChange)
            {
                timerLastChange = 0f;
                changeState();
            }


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
                    goToHouse();
                    break;

                case State.Buy:
                    goToBuy();
                    break;

                case State.Work:
                    goToWork();
                    break;
                case State.Pray:
                    goToTemple();
                    break;
                case State.Free:
                    goToCircus();
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
        State newState = (State)(((int)state + 1) % State.GetValues(typeof(State)).Length);
        
        state = newState;
    }

    public bool findBuilding(string Tag)
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(Tag);

        foreach (GameObject gameObject in taggedObjects)
        {
            if (Tag == "House") home = gameObject;
            else if (Tag == "WorkPlace") workPlace = gameObject;
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

    public void goToHouse()
    {
        if (home != null) agent.SetDestination(home.transform.position);
        else if(findBuilding("House")) agent.SetDestination(home.transform.position);
        //else GO TO THE FORUM OR TOWN CENTER
    }

    public void goToWork()
    {
        if (workPlace != null) agent.SetDestination(workPlace.transform.position);
        else if (findBuilding("WorkPlace")) agent.SetDestination(workPlace.transform.position);
        //else GO TO THE FORUM OR TOWN CENTER
    }

    public void goToBuy()
    {
        // Check needs
        agent.SetDestination(findNeed("Well"));
    }

    private void goToCircus()
    {
        throw new NotImplementedException();
    }

    private void goToTemple()
    {
        throw new NotImplementedException();
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
