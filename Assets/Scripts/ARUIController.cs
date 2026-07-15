using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.Collections;

public class ARUIController : MonoBehaviour
{
    [SerializeField] private Light focusLight;
    public GameObject focusLightObject;
  

   
    [Header("Ambient Settings")]
    [SerializeField] private Color targetAmbientColor = new Color(0.0078f, 0f, 0.482f); // #02007B
    [SerializeField] private float transitionDuration = 3f;
    [SerializeField] private bool startFromWhite = true;

    private Coroutine ambientCoroutine;



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

    public void LoadSceneByIndex(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError($"Invalid scene index: {sceneIndex}");
        }
    }

    public void ShowAllParts()
    {
        if (ARModelPartController.Instance != null)
        {
            ARModelPartController.Instance.ShowAll();
        }
    }

   public void SetCullingMask(string layerNames)
    {
        int mask = 0;

        foreach (string name in layerNames.Split(','))
        {
            string layerName = name.Trim();

            if (layerName == "Everything")
            {
                focusLight.cullingMask = ~0;
                DisablefocusLightObjects();
                return;
            }

            if (layerName == "Nothing")
            {
                focusLight.cullingMask = 0;
                DisablefocusLightObjects();
                return;
            }

            int layer = LayerMask.NameToLayer(layerName);

            if (layer == -1)
            {
                Debug.LogError($"Layer '{layerName}' does not exist.");
                continue;
            }

            mask |= 1 << layer;
        }

        focusLight.cullingMask = mask;
        EnablefocusLightObjects();
    }

    public void EnablefocusLightObjects()
    {
       SetAmbientColorMode();
    }

    public void DisablefocusLightObjects()
    {
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