using UnityEngine;

public class ButtonGroup : MonoBehaviour
{
    public string[] objectNamesToEnable;   // type names here in the inspector

    public void Activate()
    {
        if (ARModelBinder.Selector != null)            // null until image is tracked
            ARModelBinder.Selector.EnableByName(objectNamesToEnable);
    }

   

}