using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    public List<GameObject> unemployedList = new List<GameObject>();
    public List<GameObject> employedList = new List<GameObject>();

    // TODO: Make a vector for various citizen types
    public GameObject citizenPrefab;

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void initiateCity(Transform centerTransform, int nb, float radius)
    {
        for (int i = 0; i < nb; i++)
        {
            float angle = i * Mathf.PI * 2f / nb; // Calcula el ángulo
            Vector3 position = centerTransform.position + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius); // Calcula la posición

            GameObject newCitizen = Instantiate(citizenPrefab, position, Quaternion.identity); // Instancia el prefab
            unemployedList.Add(newCitizen);
        }
    }

   
    // Search a free citizen from the unemployed list
    public HumanController findFreeWorker()
    {
        foreach (GameObject citizen in unemployedList)
        {
            HumanController humanController = citizen.GetComponent<HumanController>();
            return humanController;
            
        }
        return null; 
    }

    // Changes a citizen from the unemployed list to the employed list
    public void fromUnemployedToEmployed(GameObject citizen)
    {
        unemployedList.Remove(citizen);
        employedList.Add(citizen);
    }
}