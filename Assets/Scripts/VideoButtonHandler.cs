using UnityEngine;

public class VideoButtonHandler : MonoBehaviour
{
    public int playerID;
    public int videoIndex;

    public void Play()
    {
        ARVideoManager.Instance.PlayVideo(playerID, videoIndex);
    }
}