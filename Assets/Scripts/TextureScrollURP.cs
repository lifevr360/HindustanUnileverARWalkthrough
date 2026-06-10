using UnityEngine;

public class TextureScrollURP : MonoBehaviour
{
    [Header("Assign the material here")]
    public Material targetMaterial;

    [Header("Scroll Speed")]
    public float scrollSpeedY = 0.5f;

    private Vector2 currentOffset;

    private void OnEnable()
    {
        if (targetMaterial != null)
        {
            currentOffset = targetMaterial.GetTextureOffset("_BaseMap");
        }
    }

    private void Update()
    {
        if (targetMaterial == null)
            return;

        currentOffset.y += scrollSpeedY * Time.deltaTime;

        // Keep value looping between 0 and 1
        if (currentOffset.y > 1f)
            currentOffset.y -= 1f;
        else if (currentOffset.y < 0f)
            currentOffset.y += 1f;

        targetMaterial.SetTextureOffset("_BaseMap", currentOffset);
    }
}