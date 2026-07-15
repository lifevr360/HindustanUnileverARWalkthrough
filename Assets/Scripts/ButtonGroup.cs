using UnityEngine;

public class ButtonGroup : MonoBehaviour
{
    public string[] objectNamesToEnable;   // type names here in the inspector
    public string stepManager;
    public void Activate()
    {
        if (ARModelBinder.Selector != null)            // null until image is tracked
            ARModelBinder.Selector.EnableByName(objectNamesToEnable);
    }

    public void EnableStepManager()
    {
        if(stepManager!=null)
        {
            if (ARModelBinder.Selector != null)            // null until image is tracked
                ARModelBinder.Selector.EnableObjectByName(stepManager);
        }
    }
   
      public void DisableAllStepManager()
    {
        if(stepManager!=null)
        {
            if (ARModelBinder.Selector != null)            // null until image is tracked
                ARModelBinder.Selector.DisableAllObjects();
        }
    }
   
   public void MoveLTFM(int index)
    {
        if (ARModelBinder.Mover != null)            // null until image is tracked
            ARModelBinder.Mover.MoveToStep(index);
    }

    public void ResetHULAnimation()
    {
          if (ARModelBinder.Mover != null)            // null until image is tracked
            ARModelBinder.Selector.ResetAnimation();
    }

}