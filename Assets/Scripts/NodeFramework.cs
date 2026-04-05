using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class Node
{
    public string name;

    [Header("UI")]
    public Button button;

    [Tooltip("Background shown when this node's children are visible")]
    public GameObject background;

    [Header("Hierarchy")]
    public List<Node> children = new List<Node>();

    [Header("Actions (Leaf Nodes Only)")]
    public UnityEvent onSelected;

    // NEW -------------------------
    [Header("Progress")]
    public GameObject tickIcon;
    [HideInInspector] public bool isCompleted = false;
    // -----------------------------

    public bool HasChildren
    {
        get { return children != null && children.Count > 0; }
    }
}

public class NodeFramework : MonoBehaviour
{
    [Header("Root Nodes (Always Visible)")]
    public List<Node> rootNodes = new List<Node>();

    [Header("Root Background (Optional)")]
    public GameObject rootBackground;

    [Header("Completion")]
    public GameObject completionObject;

    private Stack<Node> childStack = new Stack<Node>();
    private List<Node> activeRootChildren = null;

    // NEW -------------------------
    private Node currentRoot = null;
    // -----------------------------

    void Awake()
    {
        InitializeButtons(rootNodes);

        foreach (var root in rootNodes)
        {
            if (root.button != null)
                root.button.gameObject.SetActive(true);

            // NEW -----------------
            if (root.tickIcon != null)
                root.tickIcon.SetActive(false);
            // ---------------------
        }

        HideAllChildren();
        ShowBackground(rootBackground);

        if (completionObject != null)
            completionObject.SetActive(false);
    }

    private void InitializeButtons(List<Node> nodes)
    {
        foreach (var node in nodes)
        {
            if (node.button != null)
            {
                node.button.onClick.RemoveAllListeners();
                node.button.onClick.AddListener(() => OnNodeClicked(node));
            }

            if (node.HasChildren)
                InitializeButtons(node.children);
        }
    }

    private void OnNodeClicked(Node node)
    {
        if (node == null)
            return;

        if (node.onSelected != null && node.onSelected.GetPersistentEventCount() > 0)
        {
            node.onSelected.Invoke();
        }

        // ROOT CLICK
        if (rootNodes.Contains(node))
        {
            childStack.Clear();
            activeRootChildren = node.children;

            // NEW -----------------
            currentRoot = node;
            // ---------------------

            if (node.HasChildren)
            {
                RenderChildren(activeRootChildren);
                ShowBackground(node.background);
            }

            return;
        }

        // CHILD CLICK
        if (node.HasChildren)
        {
            childStack.Push(node);
            RenderChildren(node.children);
            ShowBackground(node.background);
        }
    }

    public void GoBack()
    {
        if (childStack.Count == 0 && activeRootChildren == null)
            return;

        // Case 2: deeper levels
        if (childStack.Count > 0)
        {
            childStack.Pop();

            if (childStack.Count > 0)
            {
                RenderChildren(childStack.Peek().children);
                ShowBackground(childStack.Peek().background);
            }
            else
            {
                RenderChildren(activeRootChildren);
                ShowBackground(GetActiveRootBackground());
            }

            return;
        }

        // Case 3: back to root
        activeRootChildren = null;

        // NEW -------------------------
        if (currentRoot != null && !currentRoot.isCompleted)
        {
            currentRoot.isCompleted = true;

            if (currentRoot.tickIcon != null)
                currentRoot.tickIcon.SetActive(true);

            CheckAllCompleted();
        }
        // -----------------------------

        RenderChildren(null);
        ShowBackground(rootBackground);
    }

    private void RenderChildren(List<Node> nodesToShow)
    {
        HideAllChildren();

        if (nodesToShow == null)
            return;

        foreach (var node in nodesToShow)
        {
            if (node.button != null)
                node.button.gameObject.SetActive(true);
        }
    }

    private void HideAllChildren()
    {
        HideChildrenRecursive(rootNodes);
    }

    private void HideChildrenRecursive(List<Node> nodes)
    {
        foreach (var node in nodes)
        {
            if (!rootNodes.Contains(node) && node.button != null)
                node.button.gameObject.SetActive(false);

            if (node.HasChildren)
                HideChildrenRecursive(node.children);
        }
    }

    private void ShowBackground(GameObject bg)
    {
        HideAllBackgrounds();

        if (bg != null)
            bg.SetActive(true);
    }

    private void HideAllBackgrounds()
    {
        HideBackgroundRecursive(rootNodes);
    }

    private void HideBackgroundRecursive(List<Node> nodes)
    {
        foreach (var node in nodes)
        {
            if (node.background != null)
                node.background.SetActive(false);

            if (node.HasChildren)
                HideBackgroundRecursive(node.children);
        }
    }

    private GameObject GetActiveRootBackground()
    {
        if (activeRootChildren == null)
            return rootBackground;

        foreach (var root in rootNodes)
        {
            if (root.children == activeRootChildren)
                return root.background;
        }

        return rootBackground;
    }

    private void CheckAllCompleted()
    {
        foreach (var root in rootNodes)
        {
            if (!root.isCompleted)
                return;
        }

        // All completed
        if (completionObject != null)
            completionObject.SetActive(true);
    }
}