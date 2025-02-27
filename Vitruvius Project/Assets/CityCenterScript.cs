using UnityEngine;

public class CityCenterScript : MonoBehaviour
{
    public static CityCenterScript Instance { get; private set; } // Singleton instance

    private bool placed = false;
    [SerializeField] private float SPAWN_RADIUS = 3f;
    [SerializeField] private int SPAWN_NUMBER = 1;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            //Optional: DontDestroyOnLoad(gameObject); // Prevent destruction on scene load
        }
        else
        {
            Debug.LogWarning("Another instance of cityCenterScript was found and destroyed.");
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Your initialization code here
    }

    // Update is called once per frame
    void Update()
    {
        // Your update logic here
    }
    public void firstTimePlaced()
    {
        this.placed = true;
        GameManager.instance.initiateCity(this.gameObject.transform, SPAWN_NUMBER, SPAWN_RADIUS);
    }

    // Get ramdnom position around the city center
    public Vector3 getRandomPositionAround()
    {
        // Generate a random point within a circle (or sphere in 3D) of the given radius
        Vector3 randomPoint = Random.insideUnitSphere * SPAWN_RADIUS;

        // Add the GameObject's position to the random point to place it around the GameObject
        Vector3 finalPosition = transform.position + randomPoint;

        return finalPosition;
    }

    // Getters

    public bool isPlaced() => placed;

    // Setters

    public void setPlaced(bool placed)
    {
        this.placed = placed;
    }
}