﻿using System;
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
    public static float defaultWindowHeight = 200f;
    public static float questionWindowHeight = 420f;
    public static float collapsedHeight = 42f;

    public Rect window;

    public bool isCollapsed;
    public string title;
    public string speakerName;
    public string question;
    public string action;
    public bool isDialogAction;
    public string friendState;
    public SerializableGuid id;
    public float x;
    public float y;
    public string text;
    public bool isThought;
    public string animTrigger;
    public DIALOGNODETYPE type;

    // For statement linking
    public SerializableGuid child_id;

    // For question linking
    public List<DialogResponse> responses;

    public DialogNode(DialogDefinition definition, string p_title, string p_text, DIALOGNODETYPE p_type, float p_x, float p_y)
    {
        isCollapsed = false;
        title = p_title;
        speakerName = "speakerName";
        question = "";
        action = "";
        isDialogAction = false;
        friendState = "";
        id = Guid.NewGuid();
        text = p_text;
        isThought = false;
        animTrigger = "";
        type = p_type;

        responses = new List<DialogResponse>();
        x = p_x;
        y = p_y;
        window = new Rect(x, y, defaultWindowWidth, defaultWindowHeight);
    }
}

[CreateAssetMenu(fileName = "New Dialog", menuName = "TGIT/Dialog", order = 3)]
public class DialogDefinition : ScriptableObject {
    public string title = "dialog_name";
    public string dialogIconID = "dialog_icon_ID";

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
            DialogNode rootNode = new DialogNode(this, "DialogRoot", "Default Text", DIALOGNODETYPE.STATEMENT, 110, 110);
            rootNodeId = rootNode.id;
            nodes[rootNode.id] = rootNode;
        }
    }
}