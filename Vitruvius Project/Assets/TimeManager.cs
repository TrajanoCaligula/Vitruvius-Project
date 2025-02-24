using System;
using UnityEngine;
using UnityEngine.AI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    public int SECONDS_PER_DAY = 10;
    public int INITIAL_YEAR = 933;  //ab urbe condita

    public float clock;
    public float deltaTime;
    public int gameSpeed;
    public float baseAgentSpeed = 5f;

    public event Action EventSacrifice; //Special event for temples, prayers...
    public event Action EventCircus;    //Special event for theaters, circus ...

    public int day;
    public int month;
    public int year;  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Opcional: Evita que el objeto se destruya al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Destruye instancias duplicadas
            return;
        }

        
    }

    public void Start()
    {
        clock = 0f;
        deltaTime = 0f;
        gameSpeed = 1;
        day = 1;
        month = 1;
        year = INITIAL_YEAR;
        UpdateAgentSpeed();
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.BackQuote)) setGameSpeed(0);
        if (Input.GetKey(KeyCode.Alpha1)) setGameSpeed(1);
        if (Input.GetKey(KeyCode.Alpha2)) setGameSpeed(2);
        if (Input.GetKey(KeyCode.Alpha3)) setGameSpeed(3);

        updateClock();
        if(month == 6)
        {
            //TODO: Change condition. When month == 6, the events are triggered
            EventCircus?.Invoke();
            EventSacrifice?.Invoke();
        }
    }

    private void updateClock()
    {
        deltaTime = Time.deltaTime * gameSpeed;
        clock += deltaTime;

        if(clock >= SECONDS_PER_DAY)
        {
            clock = 0;
            day++;
            if(day > 30)
            {
                day = 1;
                month++;
                if(month > 12)
                {
                    month = 1;
                    year++;
                }
            }
        }
    }

    // Changes the agents speeds
    private void UpdateAgentSpeed()
    {
        NavMeshAgent[] agents = FindObjectsByType<NavMeshAgent>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (NavMeshAgent agent in agents)
        {
            if (agent.agentTypeID == 0)  // Humanoid agent
            {
                agent.speed = baseAgentSpeed * gameSpeed;

            }
        }
    }

    // Getters
    public float getDeltaTime()
    {
        return deltaTime;
    }

    public float getClock()
    {
        return clock;
    }

    public int getGameSpeed()
    {
        return gameSpeed;
    }

    public bool isGamePaused()
    {
        return (gameSpeed == 0);
    }

    // Setters
    public void setGameSpeed(int newGameSpeed)
    {
        gameSpeed = newGameSpeed;
        UpdateAgentSpeed();
    }

    public void pauseGame()
    {
        gameSpeed = 0;
        UpdateAgentSpeed();
    }
}
