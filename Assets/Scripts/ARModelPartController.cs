using UnityEngine;

public class ARModelPartController : MonoBehaviour
{
    public GameObject[] parts;

    public static ARModelPartController Instance;

    void Awake()
    {
        Instance = this; // Register when spawned
    }

    void Start()
    {
       
    }

    public void ShowOnly(int index)
    {
        if (parts == null || parts.Length == 0) return;

        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].SetActive(i == index);
        }
        if (index == 4)
        {
            parts[1].SetActive(true);
        }
    }

    public void ShowAll()
    {
        foreach (GameObject part in parts)
        {
            part.SetActive(true);
        }
    }
}