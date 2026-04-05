using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ARVideoManager : MonoBehaviour
{
    public static ARVideoManager Instance;

    [Header("Video List")]
    public List<VideoClip> videoClips = new List<VideoClip>();

    private VideoPlayer currentVideoPlayer;

    private int pendingVideoIndex = -1;

    void Awake()
    {
        Instance = this;
    }

    // Called by AR object when spawned
    public void RegisterVideoPlayer(VideoPlayer vp)
    {
        currentVideoPlayer = vp;

        // If user already clicked before spawn
        if (pendingVideoIndex >= 0)
        {
            PlayVideo(pendingVideoIndex);
            pendingVideoIndex = -1;
        }
    }

    // Called by UI buttons
    public void PlayVideo(int index)
    {
        if (currentVideoPlayer == null)
        {
            // AR object not spawned yet  store request
            pendingVideoIndex = index;
            return;
        }

        if (index < 0 || index >= videoClips.Count)
        {
            Debug.LogWarning("Invalid video index");
            return;
        }
        Debug.Log("Playing video" + index);
        currentVideoPlayer.clip = videoClips[index];
        currentVideoPlayer.Play();
    }
}