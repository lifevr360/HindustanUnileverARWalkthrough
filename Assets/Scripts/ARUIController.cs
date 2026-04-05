using UnityEngine;

public class ARUIController : MonoBehaviour
{
    public void ShowPart(int index)
    {
        if (ARModelPartController.Instance != null)
        {
            ARModelPartController.Instance.ShowOnly(index);
        }
        else
        {
            Debug.Log("Model not yet tracked!");
        }
    }

    public void ShowAllParts()
    {
        if (ARModelPartController.Instance != null)
        {
            ARModelPartController.Instance.ShowAll();
        }
    }
}