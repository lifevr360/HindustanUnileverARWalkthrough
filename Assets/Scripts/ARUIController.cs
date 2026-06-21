using UnityEngine;
using UnityEngine.Rendering;

public class ARUIController : MonoBehaviour
{
    public GameObject adminLight;
    public GameObject sceneLight;

    public Color ambientColor = Color.gray;

    void Start()
    {
       adminLight.SetActive(false);
       sceneLight.SetActive(true);  
    }

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

    public void EnableAdminLights()
    {
       adminLight.SetActive(true);
       sceneLight.SetActive(false);
       SetAmbientColorMode();
    }

    public void DisableAdminLights()
    {
       adminLight.SetActive(false);
       sceneLight.SetActive(true);
       SetSkyboxMode();
    }

    public void SetAmbientColorMode()
    {
        // Change Source from Skybox to Color
        RenderSettings.ambientMode = AmbientMode.Flat;

        // Set the ambient color
        RenderSettings.ambientLight = ambientColor;

        // Refresh environment lighting
        DynamicGI.UpdateEnvironment();
    }

    public void SetSkyboxMode()
    {
        // Change Source back to Skybox
        RenderSettings.ambientMode = AmbientMode.Skybox;

        // Refresh environment lighting
        DynamicGI.UpdateEnvironment();
    }
}