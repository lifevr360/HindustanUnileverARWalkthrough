using System.Collections.Generic;
using UnityEngine;

public class ChildSelector : MonoBehaviour
{
    [Header("Groups")]
    [SerializeField] List<GameObject> groups = new();


    [Header("Individual Objects")]
    [SerializeField] List<GameObject> objects = new();


    readonly Dictionary<string, GameObject> groupByName = new();
    readonly Dictionary<string, GameObject> objectByName = new();



    void Awake()
    {
        foreach (var g in groups)
        {
            if (g)
                groupByName[g.name] = g;
        }


        foreach (var obj in objects)
        {
            if (obj)
                objectByName[obj.name] = obj;
        }
    }



    // Enable/Disable groups using multiple names
    public void EnableByName(string[] names)
    {
        var keep = new HashSet<string>(names);


        foreach (var g in groups)
        {
            if (!g) continue;

            bool on = keep.Contains(g.name);

            if (g.activeSelf != on)
                g.SetActive(on);
        }
    }



    // Enable single object using one name
    public void EnableObjectByName(string objectName)
    {
        Debug.Log($"EnableObjectByName called: {objectName}");


        if (objectByName.TryGetValue(objectName, out GameObject obj))
        {
            obj.SetActive(true);

            Debug.Log($"Object enabled: {objectName}");
        }
        else
        {
            Debug.LogWarning($"Object not found in list: {objectName}");
        }
    }
    
    // Disable all groups
    public void DisableAllGroups()
    {
        Debug.Log("Disabling all groups");

        foreach (var g in groups)
        {
            if (g != null && g.activeSelf)
            {
                g.SetActive(false);
            }
        }
    }



    // Disable all individual objects
    public void DisableAllObjects()
    {
        Debug.Log("Disabling all objects");

        foreach (var obj in objects)
        {
            if (obj != null && obj.activeSelf)
            {
                obj.SetActive(false);
            }
        }
    }


}