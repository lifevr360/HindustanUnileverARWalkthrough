using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.Collections;

public class ARUIController : MonoBehaviour
{
    public GameObject adminLight;
    public GameObject sceneLight;

   
    [Header("Ambient Settings")]
    [SerializeField] private Color targetAmbientColor = new Color(0.0078f, 0f, 0.482f); // #02007B
    [SerializeField] private float transitionDuration = 3f;
    [SerializeField] private bool startFromWhite = true;

    private Coroutine ambientCoroutine;


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
        RenderSettings.ambientMode = AmbientMode.Flat;

        if (ambientCoroutine != null)
            StopCoroutine(ambientCoroutine);

        ambientCoroutine = StartCoroutine(
            SmoothAmbientTransition(targetAmbientColor));
    }

    private IEnumerator SmoothAmbientTransition(Color targetColor)
    {
        Color startColor;

        if (startFromWhite)
        {
            startColor = Color.white;
            RenderSettings.ambientLight = startColor;
        }
        else
        {
            startColor = RenderSettings.ambientLight;
        }

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.SmoothStep(0f, 1f, elapsed / transitionDuration);

            RenderSettings.ambientLight =
                Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        RenderSettings.ambientLight = targetColor;
        DynamicGI.UpdateEnvironment();
    }

    public void SetSkyboxMode()
    {
        if (ambientCoroutine != null)
            StopCoroutine(ambientCoroutine);

        RenderSettings.ambientMode = AmbientMode.Skybox;
        DynamicGI.UpdateEnvironment();
    }
}