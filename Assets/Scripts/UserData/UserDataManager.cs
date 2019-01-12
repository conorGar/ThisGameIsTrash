using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

// Controls asyncronous saving and loading of user data streams to different platforms
public class UserDataManager : MonoBehaviour {
    public const int MAX_SLOTS = 3;

    public static UserDataManager Instance;

    [SerializeField]
    protected List<UserDataItem> userDataItems = new List<UserDataItem>();
    protected int slot = 0;
    protected bool isDirty = false;
    protected bool isWriting = false;
    protected bool isReading = false;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        // If the data is dirty, write if it's not already writing.
        if (isDirty && !isWriting)
        {
            StartCoroutine(WriteAsync());
            isWriting = true;
            isDirty = false;
        }
	}

    public void SetSlot(int p_slot)
    {
        slot = p_slot;
    }

    public int GetSlot(){
    	return slot;
    }

    public void SetDirty()
    {
        isDirty = true;
    }

    public void AddUserDataItem(UserDataItem item)
    {
        userDataItems.Add(item);
    }

    protected void SetUserItemData(JSONObject jsonObject)
    {
        // Update every data item with the data on disk
        for (int i = 0; i < userDataItems.Count; i++)
        {
            UserDataItem userDataItem = userDataItems[i];
            JSONObject userDataJsonObject = jsonObject[userDataItem.UserDataKey()].AsObject;
            userDataItem.Load(userDataJsonObject);
        }
    }

    protected JSONObject GetUserItemData()
    {
        JSONObject json_data = new JSONObject();

        // Stitch all the data items together.
        for (int i = 0; i < userDataItems.Count; i++)
        {
            UserDataItem userDataItem = userDataItems[i];
            JSONObject jsonObject = userDataItem.Save();
            json_data[userDataItem.UserDataKey()] = jsonObject;
        }

        return json_data;
    }

    public virtual IEnumerator ReadAsync(Action callback = null)
    {
        yield return null;
        Debug.Log("Data Read Complete.");
        if (callback != null)
            callback();
    }

    protected IEnumerator WhileReading()
    {
        while (isReading)
            yield return null;
    }

    protected virtual IEnumerator WriteAsync()
    {
        yield return null;
        Debug.Log("Data Write Complete.");
        isWriting = false;
    }

    protected IEnumerator WhileWriting()
    {
        while (isWriting)
            yield return null;
    }

    // helpers
    public static byte[] JsonToBytes(JSONNode json_data)
    {
        // json to string to byte[] voodoo.  spooky.
        return System.Text.Encoding.UTF8.GetBytes(json_data.SaveToBinaryBase64());
    }

    public static JSONNode BytesToJson(byte[] buffer)
    {
        string str = buffer != null ? System.Text.Encoding.UTF8.GetString(buffer) : "";
        return StringToJson(str);
    }

    public static JSONNode StringToJson(string str)
    {
        return JSON.Parse(str);
    }
}
