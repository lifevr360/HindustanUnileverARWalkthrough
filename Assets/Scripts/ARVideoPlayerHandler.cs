using UnityEngine;
using UnityEngine.Video;

public class ARVideoPlayerHandler : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    void Start()
    {
        if (ARVideoManager.Instance != null)
        {
            ARVideoManager.Instance.RegisterVideoPlayer(videoPlayer);
        }
    }
}