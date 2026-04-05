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
        int count = transform.childCount;
        parts = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            parts[i] = transform.GetChild(i).gameObject;
        }
    }

    public void ShowOnly(int index)
    {
        if (parts == null || parts.Length == 0) return;

        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].SetActive(i == index);
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