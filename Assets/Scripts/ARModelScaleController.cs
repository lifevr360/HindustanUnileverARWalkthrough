using UnityEngine;

public class ARModelScaleController : MonoBehaviour
{ [SerializeField] private float scalePercentage = 0.1f; // 10%
    [SerializeField] private float minScale = 0.2f;
    [SerializeField] private float maxScale = 5f;

    public void IncreaseScale()
    {
        Scale(1 + scalePercentage);
    }

    public void DecreaseScale()
    {
        Scale(1 - scalePercentage);
    }

    private void Scale(float multiplier)
    {
        if (ARModelBinder.CurrentModelRoot == null)
            return;

        Transform model = ARModelBinder.CurrentModelRoot;

        Vector3 newScale = model.localScale * multiplier;

        // Clamp based on X since we're scaling uniformly
        float clampedScale = Mathf.Clamp(newScale.x, minScale, maxScale);

        model.localScale = Vector3.one * clampedScale;
    }
}