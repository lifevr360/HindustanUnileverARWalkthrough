using UnityEngine;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class ARTapVideoController : MonoBehaviour
{
    public Camera arCamera;

    private GameObject currentActiveVideo;

    void Update()
    {
#if UNITY_EDITOR
        // Mouse input for Editor
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = arCamera.ScreenPointToRay(Input.mousePosition);
            ProcessRay(ray);
        }
#else
        // Touch input for Mobile
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began) return;

        // Prevent UI clicks triggering AR
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            return;

        Ray ray = arCamera.ScreenPointToRay(touch.position);
        ProcessRay(ray);
#endif
    }

    void ProcessRay(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject clicked = hit.collider.gameObject;
            Debug.Log("Click Detected: " + clicked.name);

            HandleClick(clicked);
        }
        else
        {
            Debug.Log("Raycast hit NOTHING");
        }
    }

    void HandleClick(GameObject clickedObj)
    {
        VideoPlayer vp = clickedObj.GetComponentInChildren<VideoPlayer>(true);

        if (vp == null)
        {
            Debug.Log("No VideoPlayer found on: " + clickedObj.name);
            return;
        }

        // Stop previous video properly
        if (currentActiveVideo != null && currentActiveVideo != vp.gameObject)
        {
            VideoPlayer oldVp = currentActiveVideo.GetComponent<VideoPlayer>();
            if (oldVp != null) oldVp.Stop();

            currentActiveVideo.SetActive(false);
        }

        // Activate new video
        currentActiveVideo = vp.gameObject;
        currentActiveVideo.SetActive(true);

        vp.Play();
    }
}