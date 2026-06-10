using UnityEngine;
using UnityEngine.Video;

public class ARVideoPlayerHandler : MonoBehaviour
{
    public int playerID; // Assign unique ID in Inspector

    private VideoPlayer videoPlayer;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    void Start()
    {
        if (ARVideoManager.Instance != null)
        {
            ARVideoManager.Instance.RegisterVideoPlayer(playerID, videoPlayer);
        }
    }
}