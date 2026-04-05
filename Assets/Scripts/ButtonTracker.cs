using UnityEngine;

public class ButtonTracker : MonoBehaviour
{
    // Assign size = 4 in Inspector
    public bool[] buttonClicked;

    public GameObject objectToEnable;

    private int totalButtons = 4;

    void Start()
    {
        buttonClicked = new bool[totalButtons];
    }

    // Call this from each button with its index (0,1,2,3)
    public void OnButtonClicked(int index)
    {
        buttonClicked[index] = true;

        CheckAllButtons();
    }

    void CheckAllButtons()
    {
        foreach (bool clicked in buttonClicked)
        {
            if (!clicked)
                return; // If any button is not clicked, exit
        }

        // If all are clicked
        objectToEnable.SetActive(true);
    }
}