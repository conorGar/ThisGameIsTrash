﻿﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public struct NodeResponseMenuLink
{
    public NodeResponseMenuLink(SerializableGuid p_parent_node_id, SerializableGuid p_node_id, int p_response_id)
    {
        parent_node_id = p_parent_node_id;
        node_id = p_node_id;
        response_id = p_response_id;
    }

    public SerializableGuid parent_node_id;
    public SerializableGuid node_id;
    public int response_id;
}

public struct NodeMenuLink
{
    public NodeMenuLink(SerializableGuid p_parent_node_id, SerializableGuid p_node_id)
    {
        parent_node_id = p_parent_node_id;
        node_id = p_node_id;
    }

    public SerializableGuid parent_node_id;
    public SerializableGuid node_id;
}

public class DialogEditorWindow : EditorWindow {
    [SerializeField]
    static List<DialogDefinition> dialogs = null;

    [SerializeField]
    static DialogDefinition currentDialog = null;

    static List<SerializableGuid> guidList = null;

    static Vector2 scrollPos;
    static Vector2 lastMousePos;

    [MenuItem ("Window/TGIT/Dialog Editor")]
    static void Init()
    {
        EditorStyles.textField.wordWrap = true;

        Debug.Log("Opening Dialog Editor Window");
        dialogs = new List<DialogDefinition>();
        ClearDialogs();
        RefreshDialogs();

        DialogEditorWindow window = (DialogEditorWindow)EditorWindow.GetWindow(typeof(DialogEditorWindow));
        window.Show();
    }

    static void OnEnable()
    {
        guidList = new List<SerializableGuid>();
    }

    static void ClearDialogs()
    {
        dialogs = new List<DialogDefinition>();
        currentDialog = null;
    }

    static void SelectDialog(DialogDefinition dialog)
    {
        currentDialog = dialog;
        guidList = new List<SerializableGuid>();

        foreach (var nodePair in currentDialog.nodes)
        {
            guidList.Add(nodePair.Key);
        }

        scrollPos = new Vector2(currentDialog.nodes[currentDialog.rootNodeId].x - 10, currentDialog.nodes[currentDialog.rootNodeId].y - 10);
    }

    static void RefreshDialogs()
    {
        string[] guids = AssetDatabase.FindAssets("t: DialogDefinition", null);

        for (int i = 0; i < guids.Length; i++)
        {
            var dialogDefinition = (DialogDefinition)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[i]), typeof(DialogDefinition));
            dialogs.Add(dialogDefinition);
        }

        if (dialogs.Count > 0)
            SelectDialog(dialogs[0]);
    }

    private void OnDisable()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    // MAIN WINDOW RENDER
	void OnGUI () {
        if (currentDialog != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Current Dialog: ", EditorStyles.boldLabel);
            currentDialog.title = EditorGUILayout.TextField(currentDialog.title);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Dialog Icon ID: ", EditorStyles.boldLabel);
            currentDialog.dialogIconID = EditorGUILayout.TextField(currentDialog.dialogIconID);
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.Label("NO DIALOGS EXIST!  MAKE SOME!", EditorStyles.boldLabel);
        }

        GUILayout.BeginHorizontal();
        DialogSelector();
        NewDialogButton();
        SaveDialogsButton();
        RefreshDialogsButton();
        GUILayout.EndHorizontal();

        Rect workArea = GUILayoutUtility.GetRect(10, 10000, 10, 10000);
        scrollPos = GUI.BeginScrollView(workArea, scrollPos, new Rect(0, 0, 10000, 10000));
        GUILayout.BeginArea(new Rect(0, 0, 10000, 10000));

        BeginWindows();
        if (currentDialog != null)
        {
            for (int i = 0; i < guidList.Count; i++)
            {
                DialogNode node = currentDialog.nodes[guidList[i]];
                switch (node.type)
                {
                    case DIALOGNODETYPE.STATEMENT:
                        if (!node.child_id.IsNullOrEmpty())
                            DrawNodeCurve(node.window, currentDialog.nodes[node.child_id].window);
                        break;
                    case DIALOGNODETYPE.QUESTION:
                        for (int j = 0; j < node.responses.Count; j++)
                        {
                            if (!node.responses[j].node_id.IsNullOrEmpty())
                            {
                                if (node.isCollapsed)
                                    DrawNodeCurve(node.window, currentDialog.nodes[node.responses[j].node_id].window);
                                else
                                    DrawNodeCurve(node.window, currentDialog.nodes[node.responses[j].node_id].window, j);
                            }
                        }
                        break;
                }

                node.window = GUILayout.Window(i, node.window, DrawNodeWindow, node.title);
            }
        }
        EndWindows();
        GUILayout.EndArea();
        GUI.EndScrollView();

        // Update scrolling with mouse clicks
        if (Event.current.type == EventType.MouseDrag)
        {
            Vector2 currPos = Event.current.mousePosition;
            float speed = 0f;
            switch (Event.current.button) {
                case 0: // Left Mouse
                case 1: // Right Mouse
                    speed = 2f;
                    break;
                case 2: // Middle Mouse
                    speed = 4f;
                    break;
                default:
                    // Some other mouse button
                    break;
            }

            if (Vector2.Distance(currPos, lastMousePos) < 100)
            {
                float x = lastMousePos.x - currPos.x;
                float y = lastMousePos.y - currPos.y;

                scrollPos.x += x*speed;
                scrollPos.y += y*speed;
                Event.current.Use();
            }
            lastMousePos = currPos;
        }
    }

    // NODE WINDOW RENDER
    void DrawNodeWindow(int id)
    {
        DialogNode node = currentDialog.nodes[guidList[id]];

        node.x = node.window.x;
        node.y = node.window.y;

        GUILayout.BeginHorizontal();
        GUILayout.Label("Title: ");
        node.title = WithoutSelectAll(() => EditorGUILayout.TextField(node.title));
        if (GUILayout.Button("Collapse"))
        {
            node.isCollapsed = !node.isCollapsed;
            node.window.height = 0f;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Speaker: ");
        node.speakerName = EditorGUILayout.TextField(node.speakerName);
        GUILayout.EndHorizontal();

        if (!node.isCollapsed)
        {

            //GUILayout.Label("Id: " + node.id.ToString());

            GUI.changed = false;
            GUILayout.BeginHorizontal();
            GUILayout.Label("Type: ");
            node.type = (DIALOGNODETYPE)EditorGUILayout.EnumPopup(node.type);
            GUILayout.EndHorizontal();

            if (GUI.changed) {
                node.window.height = 0f;
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Text: ");
            EditorGUI.BeginChangeCheck();
            node.text = WithoutSelectAll(() => EditorGUILayout.TextArea(node.text));
            if (EditorGUI.EndChangeCheck())
                node.window.height = 0f;

            GUILayout.EndHorizontal();

            switch (node.type)
            {
                case DIALOGNODETYPE.STATEMENT:
                    GUILayout.BeginHorizontal();
                    if (node.child_id.IsNullOrEmpty())
                    {
                        // Create
                        if (GUILayout.Button("Create Next Dialog"))
                        {
                            DialogNode new_node = new DialogNode(currentDialog, node.title, "", DIALOGNODETYPE.STATEMENT, node.window.x + 110f, node.window.y + 110f);
                            currentDialog.nodes[new_node.id] = new_node;
                            node.child_id = new_node.id;

                            guidList.Add(new_node.id);
                        }



                        // Delete
                        if (!node.id.Equals(currentDialog.rootNodeId))
                        {
                            if (GUILayout.Button("Delete Dialog"))
                            {
                                // clear any child links to the node that's about to be deleted.
                                for (int i = 0; i < guidList.Count; i++)
                                {
                                    DialogNode check_node = currentDialog.nodes[guidList[i]];
                                    if (check_node.child_id.Equals(node.id))
                                        check_node.child_id.Value = null;
                                }
                                guidList.Remove(node.id);
                                currentDialog.nodes.Remove(node.id);
                            }
                        }
                    }
                    if (GUILayout.Button("Link"))
                    {
                        // create the menu and add items to it
                        GenericMenu menu = new GenericMenu();

                        for (int j = 0; j < guidList.Count; j++)
                        {
                            DialogNode menu_node = currentDialog.nodes[guidList[j]];
                            menu.AddItem(new GUIContent(menu_node.title), menu_node.id.Equals(node.child_id), OnLinkSelected, new NodeMenuLink(node.id, menu_node.id));
                        }

                        menu.ShowAsContext();
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    node.isDialogAction = GUILayout.Toggle(node.isDialogAction, "Dialog Action?");
                    GUILayout.Label("Action: ");
                    node.action = EditorGUILayout.TextField(node.action);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Change Friend State: ");
                    node.friendState = EditorGUILayout.TextField(node.friendState);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    node.isThought = GUILayout.Toggle(node.isThought, "Is Thought?");
                    GUILayout.Label("Anim Trigger: ");
                    node.animTrigger = EditorGUILayout.TextField(node.animTrigger);
                    GUILayout.EndHorizontal();
                    break;
                case DIALOGNODETYPE.QUESTION:
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Question: ");
                    EditorGUI.BeginChangeCheck();
                    node.question = WithoutSelectAll(() => EditorGUILayout.TextArea(node.question));
                    if (EditorGUI.EndChangeCheck())
                        node.window.height = 0f;
                    GUILayout.EndHorizontal();

                    // Create
                    if (GUILayout.Button("Create Response"))
                    {
                        DialogResponse new_response = new DialogResponse("");
                        node.responses.Add(new_response);
                    }

                    for (int i = 0; i < node.responses.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("R" + i + ": ");
                        if (GUILayout.Button("X"))
                        {
                            node.responses.Remove(node.responses[i]);
                            node.window = new Rect(node.window.x, node.window.y, DialogNode.defaultWindowWidth, 0f);
                        }
                        if (GUILayout.Button("Link"))
                        {
                            // create the menu and add items to it
                            GenericMenu menu = new GenericMenu();

                            for (int j = 0; j < guidList.Count; j++)
                            {
                                DialogNode menu_node = currentDialog.nodes[guidList[j]];
                                menu.AddItem(new GUIContent(menu_node.title), menu_node.id.Equals(node.responses[i].node_id), OnResponseLinkSelected, new NodeResponseMenuLink(node.id, menu_node.id, i));
                            }

                            menu.ShowAsContext();
                        }
                        GUILayout.EndHorizontal();

                        EditorGUI.BeginChangeCheck();
                        node.responses[i].text = WithoutSelectAll(() => EditorGUILayout.TextArea(node.responses[i].text));
                        if (EditorGUI.EndChangeCheck())
                            node.window.height = 0f;
                    }
                    break;
            }
        }

        GUI.DragWindow();
    }

    // DrawNodeCurve example from this post https://forum.unity.com/threads/simple-node-editor.189230/ and modified to handle an offset based on response number so they are more readable.
    void DrawNodeCurve(Rect start, Rect end, int responseNumber = -1)
    {
        Vector3 startPos;
        if (responseNumber >= 0)
        {
           startPos = new Vector3(start.x + start.width, (start.y + start.height / 2) + responseNumber * 20f );
        }
        else
        {
           startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        }
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);

        for (int i = 0; i < 3; i++)
        {// Draw a shadow
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        }

        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }

    void RefreshDialogsButton()
    {
        if (GUILayout.Button("Refresh Dialogs"))
        {
            ClearDialogs();
            RefreshDialogs();
        }
    }

    void NewDialogButton()
    {
        if (GUILayout.Button("Create New Dialog Node"))
        {
            DialogNode new_node = new DialogNode(currentDialog, "NewDialog", "Child Dialog", DIALOGNODETYPE.STATEMENT, scrollPos.x + 10f, scrollPos.y + 10f);
            currentDialog.nodes[new_node.id] = new_node;
            guidList.Add(new_node.id);
        }
    }

    void DialogSelector()
    {
        if (GUILayout.Button("Select Dialog Asset"))
        {
            // create the menu and add items to it
            GenericMenu menu = new GenericMenu();

            for (int i = 0; i < dialogs.Count; i++)
            {
                menu.AddItem(new GUIContent(dialogs[i].title), dialogs[i].title == currentDialog.title, OnDialogSelected, i);
            }
            
            menu.ShowAsContext();
        }
    }

    void SaveDialogsButton()
    {
        if (GUILayout.Button("Save Assets"))
        {
            Debug.Log("Saving Dialogs to Asset Database.");
            for (int i = 0; i < dialogs.Count; i++)
            {
                EditorUtility.SetDirty(dialogs[i]);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    void OnDialogSelected(object index)
    {
        int i = (int)index;
        DialogDefinition dialog = dialogs[i];
        SelectDialog(dialog);
    }

    void OnResponseLinkSelected(object obj)
    {
        // When the menu item is chosen, link up the response to the node selected.
        var link = (NodeResponseMenuLink)obj;
        DialogNode parent_node = currentDialog.nodes[link.parent_node_id];
        DialogNode node = currentDialog.nodes[link.node_id];
        parent_node.responses[link.response_id].node_id = node.id;
    }

    void OnLinkSelected(object obj)
    {
        // When the menu item is chosen, link up the parent node to the child node selected.
        var link = (NodeMenuLink)obj;
        DialogNode parent_node = currentDialog.nodes[link.parent_node_id];
        DialogNode node = currentDialog.nodes[link.node_id];
        parent_node.child_id = node.id;

        Debug.Log("Parent_node.id : " + parent_node.id + "\nChild_node.id : " + node.id);
    }

    // Insane hack from a smart person because Unity loves to select all text when you use EditorGUILayouts... Source: https://stackoverflow.com/questions/44097608/how-can-i-stop-immediate-gui-from-selecting-all-text-on-click
    private T WithoutSelectAll<T>(Func<T> guiCall)
    {
        bool preventSelection = (Event.current.type == EventType.MouseDown);

        Color oldCursorColor = GUI.skin.settings.cursorColor;

        if (preventSelection)
            GUI.skin.settings.cursorColor = new Color(0, 0, 0, 0);

        T value = guiCall();

        if (preventSelection)
            GUI.skin.settings.cursorColor = oldCursorColor;

        return value;
    }
}