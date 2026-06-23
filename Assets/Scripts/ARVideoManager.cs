using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

// Lightweight pairing of "which player" + "which video".
// Defined at top level so both the manager and the button handler can use it.
[System.Serializable]
public struct VideoAssignment
{
    public int playerID;
    public int videoIndex;
}

public class ARVideoManager : MonoBehaviour
{
    public static ARVideoManager Instance;

    [Header("Video List")]
    public List<VideoClip> videoClips = new List<VideoClip>();

    private Dictionary<int, VideoPlayer> videoPlayerMap = new Dictionary<int, VideoPlayer>();

    // Tracks every player currently meant to be active (was a single int before).
    private HashSet<int> currentActivePlayerIDs = new HashSet<int>();

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

    // -----------------------------------------------------------------
    // Single video. Kept for backward compatibility — just wraps the
    // multi-play path with a one-item list.
    // -----------------------------------------------------------------
    public void PlayVideo(int playerID, int videoIndex, bool stopOthers = true)
    {
        var single = new List<VideoAssignment>
        {
            new VideoAssignment { playerID = playerID, videoIndex = videoIndex }
        };

        PlayVideos(single, stopOthers);
    }

    // -----------------------------------------------------------------
    // Play several videos at once across several players.
    // stopOthers = true keeps your old behaviour (everything not in this
    // group is stopped + hidden). Set it false if you want this group to
    // play on top of whatever is already running.
    // -----------------------------------------------------------------
    public void PlayVideos(List<VideoAssignment> assignments, bool stopOthers = true)
    {
        if (assignments == null || assignments.Count == 0)
        {
            Debug.LogWarning("No video assignments provided");
            return;
        }

        // Collect the player IDs this call wants active.
        HashSet<int> activeIDs = new HashSet<int>();
        foreach (var a in assignments)
        {
            activeIDs.Add(a.playerID);
        }

        if (stopOthers)
        {
            DisableAllExcept(activeIDs);
        }

        currentActivePlayerIDs = activeIDs;

        // Now start each assigned video on its player.
        foreach (var a in assignments)
        {
            if (!videoPlayerMap.ContainsKey(a.playerID))
            {
                Debug.LogWarning("No VideoPlayer found for ID: " + a.playerID);
                continue;
            }

            if (a.videoIndex < 0 || a.videoIndex >= videoClips.Count)
            {
                Debug.LogWarning($"Invalid video index {a.videoIndex} for player {a.playerID}");
                continue;
            }

            VideoPlayer vp = videoPlayerMap[a.playerID];

            vp.gameObject.SetActive(true); // ensure it's enabled
            vp.clip = videoClips[a.videoIndex];
            vp.Play();

            Debug.Log($"Playing video {a.videoIndex} on player {a.playerID}");
        }
    }

    // Now takes a set of IDs to keep alive instead of a single ID.
    private void DisableAllExcept(HashSet<int> activeIDs)
    {
        foreach (var pair in videoPlayerMap)
        {
            if (!activeIDs.Contains(pair.Key))
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
}