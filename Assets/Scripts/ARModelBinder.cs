using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARModelBinder : MonoBehaviour
{
    public static ChildSelector Selector { get; private set; }
    public static Transform CurrentModelRoot { get; private set; }

    [SerializeField] ARTrackedImageManager manager;

    void OnEnable()  => manager.trackedImagesChanged += OnChanged;
    void OnDisable() => manager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs e)
    {
        Debug.Log("Image Tracked");
        foreach (var img in e.added)
        {
            // searches the whole subtree, so it finds ChildSelector even on a grandchild
            var sel = img.GetComponentInChildren<ChildSelector>(true);
            Debug.Log("Selector" +sel);
            if (sel)
            {
                Selector = sel;
                CurrentModelRoot = sel.transform; // OffsetObject
            }
        }
    }
}