using System;
using System.Collections.Generic;
using UnityEngine;

public enum DIALOGNODETYPE
{
    NONE = 0,
    STATEMENT,
    QUESTION
}

// Serializable template instance to save DialogsNodes.
[Serializable]
public class DialogNodeDictionary : SerializableDictionary<SerializableGuid, DialogNode> {}

[Serializable]
public class DialogResponse
{
    public DialogResponse(string p_text)
    {
        text = p_text;
    }

    public string text;
    public SerializableGuid node_id;
}


[Serializable]
public class DialogNode
{
    public static float defaultWindowWidth = 270f;
    public static float defaultWindowHeight = 250f;
    public static float collapsedHeight = 42f;

    public Rect window;

    public bool isCollapsed;
    public string title;
    public SerializableGuid id;
    public float x;
    public float y;
    public string text;
    public DIALOGNODETYPE type;

    public SerializableGuid parent_id;
    // For statement linking
    public SerializableGuid child_id;

    // For question linking
    public List<DialogResponse> responses;

    public DialogNode(DialogDefinition definition, string p_title, string p_text, DIALOGNODETYPE p_type, DialogNode p_parent, float p_x, float p_y)
    {
        isCollapsed = false;
        title = p_title;
        id = Guid.NewGuid();
        text = p_text;
        type = p_type;
        if (p_parent != null)
            parent_id = p_parent.id;

        responses = new List<DialogResponse>();
        x = p_x;
        y = p_y;
        window = new Rect(x, y, defaultWindowWidth, defaultWindowHeight);
    }
}

[CreateAssetMenu(fileName = "New Dialog", menuName = "TGIT/Dialog", order = 3)]
public class DialogDefinition : ScriptableObject {
    public string title = "dialog_name";

    [SerializeField]
    public DialogNodeDictionary nodeStore = DialogNodeDictionary.New<DialogNodeDictionary>();
    public Dictionary<SerializableGuid, DialogNode> nodes
    {
        get { return nodeStore.dictionary; }
    }

    public SerializableGuid rootNodeId;

	void Awake () {
        Debug.Log("DialogDefinition Awake");

        if (rootNodeId.IsNullOrEmpty())
        {
            DialogNode rootNode = new DialogNode(this, "DialogRoot", "Default Text", DIALOGNODETYPE.STATEMENT, null, 110, 110);
            rootNodeId = rootNode.id;
            nodes[rootNode.id] = rootNode;
        }
    }
}
