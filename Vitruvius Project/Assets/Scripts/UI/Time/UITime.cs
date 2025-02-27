using UnityEngine;
using UnityEngine.UIElements;

public class UITime : MonoBehaviour
{
    public UIDocument uiDocument;

    // Access the elements by name
    private Button pauseButton;
    private Button speed1Button;
    private Button speed2Button;
    private Button speed3Button;
    private Label yearLabel;
    private Label monthLabel;
    private Label dayLabel;

    // Internal variables
    private int gameSpeed;


    void Start()
    {
        uiDocument = this.gameObject.GetComponentInChildren<UIDocument>();

        // Get the root VisualElement
        VisualElement root = uiDocument.rootVisualElement;

        // Access the elements by name
        pauseButton = root.Q<Button>("pauseButton");
        speed1Button = root.Q<Button>("speed1Button");
        speed2Button = root.Q<Button>("speed2Button");
        speed3Button = root.Q<Button>("speed3Button");
        yearLabel = root.Q<Label>("yearLabel");
        monthLabel = root.Q<Label>("monthLabel");
        dayLabel = root.Q<Label>("dayLabel");

        // Add event listeners to the buttons
        pauseButton.clicked += PauseButtonClicked;
        speed1Button.clicked += Speed1ButtonClicked;
        speed2Button.clicked += Speed2ButtonClicked;
        speed3Button.clicked += Speed3ButtonClicked;

        UpdateTime(933, 1, 1); //Initial values.
        HighlightButton(speed1Button);
    }

    public void UpdateTime(int day, int month, int year)
    {
        yearLabel.text = year.ToString();
        monthLabel.text = month.ToString();
        dayLabel.text = day.ToString();
    }

    // Buttons
    private void PauseButtonClicked()
    {
        TimeManager.Instance.setGameSpeed(0);
    }

    private void Speed1ButtonClicked()
    {
        TimeManager.Instance.setGameSpeed(1);
    }

    private void Speed2ButtonClicked()
    {
        TimeManager.Instance.setGameSpeed(2);
    }

    private void Speed3ButtonClicked()
    {
        TimeManager.Instance.setGameSpeed(3);
    }

    public void updateButtons(int currentGameSpeed)
    {
        // Reset all button colors
        ResetButtonColors();

        // Highlight the selected button
        switch (currentGameSpeed)
        {
            case 0:
                HighlightButton(pauseButton);
                break;
            case 1:
                HighlightButton(speed1Button);
                break;
            case 2:
                HighlightButton(speed2Button);
                break;
            case 3:
                HighlightButton(speed3Button);
                break;
        }
    }

    private void HighlightButton(Button button)
    {
        if (button != null)
        {
            button.style.backgroundColor = new StyleColor(Color.yellow); // Change to your desired highlight color
        }
    }

    private void ResetButtonColors()
    {
        if (pauseButton != null) pauseButton.style.backgroundColor = StyleKeyword.Null;
        if (speed1Button != null) speed1Button.style.backgroundColor = StyleKeyword.Null;
        if (speed2Button != null) speed2Button.style.backgroundColor = StyleKeyword.Null;
        if (speed3Button != null) speed3Button.style.backgroundColor = StyleKeyword.Null;
    }
}