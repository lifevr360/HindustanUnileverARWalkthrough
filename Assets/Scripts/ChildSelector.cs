using System.Collections.Generic;
using UnityEngine;

public class ChildSelector : MonoBehaviour
{
    [SerializeField] List<GameObject> groups = new();   // drag the group objects here

    readonly Dictionary<string, GameObject> byName = new();

    void Awake()
    {
        foreach (var g in groups)
            if (g) byName[g.name] = g;
    }

    // Called by ButtonGroup with the typed names.
    public void EnableByName(string[] names)
    {
        Debug.Log($"EnableByName called with {names.Length} name(s): {string.Join(", ", names)}");
        Debug.Log($"groups list has {groups.Count} entr(ies)");

        var keep = new HashSet<string>(names);

        foreach (var g in groups)
        {
            if (!g) { Debug.LogWarning("A groups entry is NULL — reference was lost"); continue; }
            bool on = keep.Contains(g.name);
            Debug.Log($"group '{g.name}'  ->  {(on ? "ON" : "off")}");
            if (g.activeSelf != on) g.SetActive(on);
        }
    }

 
}