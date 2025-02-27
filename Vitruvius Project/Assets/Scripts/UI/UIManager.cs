using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public VisualTreeAsset panelNegroUXML;
    private VisualElement panelNegro;

    private GameObject building;

    // Panel Elements
    private Label textoNumero;
    private Button addButton;
    private Button subButton;

    // Building Info
    private int numberOfWorkers = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        building = this.gameObject;
        var root = GetComponent<UIDocument>().rootVisualElement;
        var visualTree = panelNegroUXML.CloneTree();
        root.Add(visualTree);

        panelNegro = root.Q<VisualElement>("panelNegro");
        textoNumero = root.Q<Label>("textoNumero");
        addButton = root.Q<Button>("addButton");
        subButton = root.Q<Button>("subButton");

        addButton.clicked += searchWorker;
        subButton.clicked += freeWorker;
    }

    void Update()
    {
        if (IsPanelOpen())
        {
            UpdatePanelContent();
        }
    }

    void UpdatePanelContent()
    {
        textoNumero.text = "Trabajadores: " + numberOfWorkers;
    }

    bool IsPanelOpen()
    {
        return panelNegro.style.display == DisplayStyle.Flex;
    }

    // Setters
    public void updateWorkers(int numberOfWorkers)
    {
        this.numberOfWorkers = numberOfWorkers;
    }

    public void updateBuilding(GameObject building)
    {
        this.building = building;
    }

    // Visibility
    public void mostrarPanelNegro()
    {
        textoNumero.text = "Trabajadores: " + numberOfWorkers;
        panelNegro.style.display = DisplayStyle.Flex;
    }

    public void ocultarPanelNegro()
    {
        panelNegro.style.display = DisplayStyle.None;
    }

    // Buttons
    void searchWorker()
    {
        HumanController worker = GameManager.instance.findFreeWorker();
        if(worker != null)
        {
            worker.workPlace = building;
            worker.addJobAndScript();
        }
    }

    void freeWorker()
    {
        Debug.Log("Botón Dos clicado");
        // Añade aquí la lógica para el botón dos
    }
}