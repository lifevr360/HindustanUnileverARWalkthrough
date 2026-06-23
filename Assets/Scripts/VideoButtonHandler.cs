using System.Collections.Generic;
using UnityEngine;

public class VideoButtonHandler : MonoBehaviour
{
    [Header("Single video (used only if Assignments list is empty)")]
    public int playerID;
    public int videoIndex;

    [Header("Multiple simultaneous videos (optional)")]
    [Tooltip("Add 2+ entries here to fire several players at once from this one button.")]
    public List<VideoAssignment> assignments = new List<VideoAssignment>();

    [Header("Behaviour")]
    [Tooltip("If true, players NOT in this button's set are stopped & hidden when pressed.")]
    public bool stopOtherPlayers = true;

    public void Play()
    {
        if (ARVideoManager.Instance == null)
        {
            Debug.LogWarning("ARVideoManager.Instance is null");
            return;
        }

        // If the list has entries, play them all together.
        // Otherwise fall back to the old single playerID/videoIndex setup.
        if (assignments != null && assignments.Count > 0)
        {
            ARVideoManager.Instance.PlayVideos(assignments, stopOtherPlayers);
        }
        else
        {
            ARVideoManager.Instance.PlayVideo(playerID, videoIndex, stopOtherPlayers);
        }
    }
}