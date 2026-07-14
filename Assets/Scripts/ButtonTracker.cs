using UnityEngine;
using UnityEngine.Events;

public class ButtonTracker : MonoBehaviour
{
    public bool[] buttonClicked;

    public GameObject objectToEnable;


    [Header("Function To Call After All Buttons Clicked")]
    public UnityEvent onAllButtonsClicked;


    public int totalButtons = 4;


    void Start()
    {
        buttonClicked = new bool[totalButtons];
    }


    // Call this from each button with its index (0,1,2,3)
    public void OnButtonClicked(int index)
    {
        if (index < 0 || index >= buttonClicked.Length)
        {
            Debug.LogWarning("Invalid button index");
            return;
        }


        buttonClicked[index] = true;

        CheckAllButtons();
    }



    void CheckAllButtons()
    {
        foreach (bool clicked in buttonClicked)
        {
            if (!clicked)
                return;
        }


        // Enable object if assigned
        if(objectToEnable != null)
            objectToEnable.SetActive(true);


        // Call any linked function
        onAllButtonsClicked.Invoke();
    }
}