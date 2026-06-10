using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ARVideoManager : MonoBehaviour
{
    public static ARVideoManager Instance;

    [Header("Video List")]
    public List<VideoClip> videoClips = new List<VideoClip>();

    private Dictionary<int, VideoPlayer> videoPlayerMap = new Dictionary<int, VideoPlayer>();

    // NEW -------------------------
    private int currentActivePlayerID = -1;
    // -----------------------------

    void Awake()
    {
        Instance = this;

        // Optional safety reset
        foreach (var vp in FindObjectsOfType<VideoPlayer>())
        {
            vp.gameObject.SetActive(false);
        }
    }

    public void RegisterVideoPlayer(int id, VideoPlayer vp)
    {
        if (!videoPlayerMap.ContainsKey(id))
        {
            videoPlayerMap.Add(id, vp);
            vp.gameObject.SetActive(false); // disable initially
            Debug.Log("Registered VideoPlayer ID: " + id);
        }
    }

    public void PlayVideo(int playerID, int videoIndex)
    {
        if (!videoPlayerMap.ContainsKey(playerID))
        {
            Debug.LogWarning("No VideoPlayer found for ID: " + playerID);
            return;
        }

        if (videoIndex < 0 || videoIndex >= videoClips.Count)
        {
            Debug.LogWarning("Invalid video index");
            return;
        }

        // NEW -------------------------
        DisableAllExcept(playerID);
        currentActivePlayerID = playerID;
        // -----------------------------

        VideoPlayer vp = videoPlayerMap[playerID];

        vp.gameObject.SetActive(true); // ensure it's enabled
        vp.clip = videoClips[videoIndex];
        vp.Play();

        Debug.Log($"Playing video {videoIndex} on player {playerID}");
    }

    // NEW -------------------------
    private void DisableAllExcept(int activeID)
    {
        foreach (var pair in videoPlayerMap)
        {
            if (pair.Key != activeID)
            {
                VideoPlayer vp = pair.Value;

                if (vp != null)
                {
                    vp.Stop();
                    vp.gameObject.SetActive(false);
                }
            }
        }
    }
    // -----------------------------
}