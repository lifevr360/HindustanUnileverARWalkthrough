using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARModelBinder : MonoBehaviour
{
    public static ChildSelector Selector { get; private set; }
    public static Transform CurrentModelRoot { get; private set; }
    public static StepMover Mover { get; private set; }

    [SerializeField] ARTrackedImageManager manager;

    void OnEnable()  => manager.trackedImagesChanged += OnChanged;
    void OnDisable() => manager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs e)
    {
        foreach (var img in e.added)
        {
            var sel = img.GetComponentInChildren<ChildSelector>(true);
            if (sel)
            {
                Selector = sel;
                CurrentModelRoot = sel.transform; // OffsetObject
            }

            var mover = img.GetComponentInChildren<StepMover>(true);
            if (mover)
            {
                Mover = mover;
                Debug.Log("StepMover bound: " + mover.name);
            }
        }
    }
}