using UnityEngine;
using UnityEngine.AI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    public float clock;
    public int gameSpeed;
    public float baseAgentSpeed = 5f;
    private float baseAgentAcceleration = 8f;

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
        gameSpeed = 1;
        UpdateAgentSpeed();
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            gameSpeed = 1;
            UpdateAgentSpeed();
        }
        if (Input.GetKey(KeyCode.L))
        {
            gameSpeed = 3;
            UpdateAgentSpeed();
        }
        clock = Time.deltaTime * gameSpeed;
    }

    // Getters
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

    private void UpdateAgentSpeed()
    {
        NavMeshAgent[] agents = FindObjectsByType<NavMeshAgent>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (NavMeshAgent agent in agents)
        {
            if (agent.agentTypeID == 0)  // Humanoid agent
            {
                agent.speed = baseAgentSpeed * gameSpeed;
                agent.acceleration = baseAgentAcceleration * gameSpeed;
            }
        }
    }
}
